using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;

namespace com.businesscentral
{
    public static class PayPalPaymentWebHook
    {
        static string TAG = "PayPalPaymentWebHook";
        [FunctionName("PayPalPaymentWebHook")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("Request notified");

            if (!req.Method.Equals("GET") && !req.Method.Equals("POST"))
                return new BadRequestObjectResult("PayPalPaymentWebHook - Unexpected " + req.Method + " request");

            // Validation token for webhook registration
            //  reply token to accept webhook subscription
            var validationToken = req.Query["validationToken"].ToString();
            if (!String.IsNullOrEmpty(validationToken))
            {
                dynamic data = JsonConvert.SerializeObject(validationToken);
                return new ContentResult { Content = data, ContentType = "application/json; charset=utf-8", StatusCode = 200 };
            }

            // Load configuration
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var config = new ConnectorConfig(configBuilder);

            var composer = new MessageComposer(config, log);
            var messenger = new MessageConnector(config, log);
            var centralConnector = new BusinessCentralConnector(config, log);

            // Get webhook and process it 
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(TAG + " Request notified from webhook : " + requestBody);

            // Message sent
            var messageEmail = messenger.SendTestMail(requestBody);
            log.LogInformation(String.Format(TAG + " SMS/Email Message result : {0}", messageEmail.Status));

            // Get webhook and process it 
            var ev = !String.IsNullOrEmpty(requestBody) ? JsonConvert.DeserializeObject<PayPalWebHook>(requestBody) : null;
            log.LogInformation(TAG + " Request notified from webhook : " + requestBody);

            // If the webhook signals a payment
            if (ev.EventType.Equals("PAYMENT.CAPTURE.COMPLETED"))
            {
                log.LogInformation(String.Format(TAG + " PAYMENT.CAPTURE.COMPLETED : {0}", ev.Resource.Id));
                var order = await centralConnector.GetPayPalOrderByExternalDocumentNumber(ev.Resource.Id);
                if (order != null)
                {
                    log.LogInformation(String.Format(TAG + " GetPayPalOrderByExternalDocumentNumber : {0}", order.Number));
                    await centralConnector.UpdatePayPalOrderById(order, order.ExternalDocumentNumber, config.paymentTermsIdPayPalPaid);
                    log.LogInformation(String.Format(TAG + " Order updated : {0}", order.Number));

                    var html = composer.DataBindEmailPayment(order);
                    await messenger.SendAdminMail(html);
                }
                else
                    log.LogError(String.Format(TAG + " PAYMENT.CAPTURE.COMPLETED Order not found {0}", ev.Resource.Id));
            }
            return new StatusCodeResult(200);
        }
    }

}
