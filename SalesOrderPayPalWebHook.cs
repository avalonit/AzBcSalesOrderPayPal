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
            if (!req.Method.Equals("GET") && !req.Method.Equals("POST"))
                return new BadRequestObjectResult("Unexpected " + req.Method + " request");

            // Validation token for webhook registration
            //  reply token to accept webhook subscription
            var validationToken = req.Query["validationToken"].ToString();
            if (validationToken != null)
            {
                dynamic data = JsonConvert.SerializeObject(validationToken);
                return new ContentResult { Content = data, ContentType = "application/json; charset=utf-8", StatusCode = 200 };
            }

            // Get webhook and process it 
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var ev = !String.IsNullOrEmpty(requestBody) ? JsonConvert.DeserializeObject<WebHookEvents>(requestBody) : null;
            log.LogInformation(String.Format("Orders notified from webhook: {0}"), ev.ToString());

            // Load configuration
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var config = new ConnectorConfig(configBuilder);
            var centraConnector = new BusinessCentralConnector(config);

            if (!(ev == null || ev.Value == null || ev.Value.Count == 0))
            {
                foreach (var wb_event in ev.Value.Where(a => a.ChangeType.Equals("updated")))
                {
                    log.LogInformation(String.Format("Processing order : {0}"), wb_event?.Resource);

                    // Business Central is queried to get order and sale agent detail 
                    var order = await centraConnector.GetPayPalOrderByWebhook(wb_event);
                    log.LogInformation(String.Format("Order : {0} of customer {1}"), order?.Number, order?.CustomerNumber);

                    var customer = await centraConnector.GetCustomerByOrder(order);
                    log.LogInformation(String.Format("Customer : {0}"), customer?.Number);

                    // Message is composed
                    var composer = new MessageComposer();
                    var messageText = composer.DataBindMessage(order);
                    log.LogInformation(String.Format("Message : {0}"), messageText);

                    // Message sent
                    var twilioMessage = new TwilioConnector();
                    var message = twilioMessage.SendMessage(messageText, customer, config);
                    log.LogInformation(String.Format("Message result : {0}"), message.Status);

                }
                return new StatusCodeResult(200);
            }
            return new BadRequestObjectResult("Bad request");
        }
    }

}
