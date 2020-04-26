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
using System.Linq;

namespace com.businesscentral
{
    public static class SalesOrderPayPalWebHook
    {
        [FunctionName("SalesOrderPayPalWebHook")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("Request notified");

            if (!req.Method.Equals("GET") && !req.Method.Equals("POST"))
                return new BadRequestObjectResult("Unexpected " + req.Method + " request");

            // Validation token for webhook registration
            //  reply token to accept webhook subscription
            var validationToken = req.Query["validationToken"].ToString();
            if (!String.IsNullOrEmpty(validationToken))
            {
                dynamic data = JsonConvert.SerializeObject(validationToken);
                return new ContentResult { Content = data, ContentType = "application/json; charset=utf-8", StatusCode = 200 };
            }

            // Get webhook and process it 
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var ev = !String.IsNullOrEmpty(requestBody) ? JsonConvert.DeserializeObject<WebHookEvents>(requestBody) : null;
            log.LogInformation("Request notified from webhook : " + requestBody);

            // Load configuration
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var config = new ConnectorConfig(configBuilder);
            var centralConnector = new BusinessCentralConnector(config, log);
            var composer = new MessageComposer(config, log);
            var messenger = new MessageConnector(config, log);
            var paypal = new PayPalConnector(config, log);

            if (!(ev == null || ev.Value == null || ev.Value.Count == 0))
            {
                log.LogInformation(String.Format("Found {0} events", ev.Value.Count.ToString()));
                var wb_UpdatedEvents = ev.Value.Where(a => a.ChangeType.Equals("updated")).ToList();
                log.LogInformation(String.Format("Found {0} update events", wb_UpdatedEvents.Count().ToString()));
                foreach (var wb_event in wb_UpdatedEvents)
                {
                    log.LogInformation(String.Format("Processing event : {0}", wb_event?.Resource));

                    // Business Central is queried to get order and sale agent detail 
                    var paypalPendingOrder = await centralConnector.GetPayPalOrderByWebhook(wb_event);
                    if (paypalPendingOrder != null)
                    {
                        log.LogInformation(String.Format("Order : {0} of customer {1}", paypalPendingOrder?.Number, paypalPendingOrder?.CustomerNumber));

                        var customer = await centralConnector.GetCustomerByOrder(paypalPendingOrder);
                        log.LogInformation(String.Format("Customer : {0}", customer?.Number));

                        // Create PayPal request
                        var paypalResponse = await paypal.CreateRequest(paypalPendingOrder, customer);

                        // Update order status as PayPal-pending payment
                        await centralConnector.UpdatePayPalOrderById(paypalPendingOrder, paypalResponse.Id, config.paymentTermsIdPayPal);

                        // Message is composed
                        var messageText = composer.DataBindMessage(paypalPendingOrder);
                        var messageHtml = composer.DataBindEmail(paypalPendingOrder, paypalResponse);
                        log.LogInformation(String.Format("Message : {0}", messageText));

                        // Message sent
                        //var messageSms = messenger.SendSMS(messageText, customer, config);
                        var messageEmail = messenger.SendCustomerMail(messageHtml, customer);
                        log.LogInformation(String.Format("SMS/Email Message result : {0}", messageEmail.Status));
                    }
                }
                return new StatusCodeResult(200);
            }
            return new StatusCodeResult(200);
        }
    }

}
