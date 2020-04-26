using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace com.businesscentral
{
    public static class AzPayPal
    {
        [FunctionName("AzPayPal")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("Request notified");

            // Load configuration
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var config = new ConnectorConfig(configBuilder);
            if (!req.Method.Equals("GET"))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var orderID = req.Query["orderID"].ToString();
            if (String.IsNullOrEmpty(orderID))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var composer = new MessageComposer(config, log);
            var html = composer.DataBindPayPalPage(orderID);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(html);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
