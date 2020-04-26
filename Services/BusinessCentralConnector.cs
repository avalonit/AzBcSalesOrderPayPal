using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace com.businesscentral
{
    public class BusinessCentralConnector
    {
        private ConnectorConfig config;
        private ILogger log;
        private string ApiWebHookEndPoint = string.Empty;
        private string ApiEndPoint = string.Empty;
        private string AuthInfo = string.Empty;
        public BusinessCentralConnector(ConnectorConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
            this.ApiWebHookEndPoint = String.Format("https://api.businesscentral.dynamics.com/{0}/{1}/", config.apiVersion1, config.tenant);
            this.ApiEndPoint = String.Format("https://api.businesscentral.dynamics.com/{0}/{1}/api/{2}/companies({3})/",
                                    config.apiVersion1, config.tenant, config.apiVersion2, config.companyID);

            this.AuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(config.authInfo));
        }
        public async Task<SalesOrder> GetPayPalOrderByWebhook(WebHookEvent ev)
        {
            SalesOrder order = null;
            var apiEndPoint = this.ApiWebHookEndPoint + ev.Resource;
            log.LogInformation("GetPayPalOrderByWebhook with endpoint " + apiEndPoint);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);
                var responseMessage = await httpClient.GetAsync(apiEndPoint);
                if (responseMessage.IsSuccessStatusCode)
                {
                    order = JsonConvert.DeserializeObject<SalesOrder>(await responseMessage.Content.ReadAsStringAsync());
                    //Only allow PAYPAL payments not yet paid and not being processed to be processed
                    if (order != null)
                        order = (order.PaymentTermsId.ToString().Equals(config.paymentTermsIdPayPal) 
                        && order.ExternalDocumentNumber.Equals(String.Empty)) ? order : null; 
                }
            }
            return order;
        }

        public async Task<SalesOrder> GetPayPalOrderById(string saleOrderId)
        {
            var apiEndPoint = String.Format("{0}/salesOrders({1})", this.ApiWebHookEndPoint, saleOrderId);
            log.LogInformation("GetPayPalOrderById with endpoint " + apiEndPoint);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);
                var responseMessage = await httpClient.GetAsync(apiEndPoint);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var order = JsonConvert.DeserializeObject<SalesOrder>(await responseMessage.Content.ReadAsStringAsync());
                    return order;
                }
            }
            return null;
        }

        public async Task<SalesOrder> UpdatePayPalOrderById(SalesOrder order, string ExternalDocumentNumber, string newPaymentTermId)
        {
            var apiEndPoint = String.Format("{0}/salesOrders({1})", this.ApiEndPoint, order.Id.ToString());
            log.LogInformation("UpdatePayPalOrderById with endpoint " + apiEndPoint);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);
                httpClient.DefaultRequestHeaders.Add("If-Match", order.OdataEtag);
                var salesOrderUpdate = new SalesOrderUpdate()
                {
                    PaymentTermsId = Guid.Parse(newPaymentTermId),
                    ExternalDocumentNumber = ExternalDocumentNumber,
                };
                var jsonObject = JsonConvert.SerializeObject(salesOrderUpdate);
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                var responseMessage = await httpClient.PatchAsync(apiEndPoint, content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var newOrder = JsonConvert.DeserializeObject<SalesOrder>(await responseMessage.Content.ReadAsStringAsync());
                    return newOrder;
                }
            }
            return null;
        }

        public async Task<SalesOrder> GetPayPalOrderByExternalDocumentNumber(string externalDocumentNumber)
        {
            SalesOrder order = null;
            var query = String.Format("salesOrders?$filter=externalDocumentNumber eq '{0}'", externalDocumentNumber);

            var apiEndPoint = this.ApiEndPoint + query;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);
                var responseMessage = await httpClient.GetAsync(apiEndPoint);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var orders = JsonConvert.DeserializeObject<SalesOrders>(await responseMessage.Content.ReadAsStringAsync());
                    if (orders != null && orders.Value != null && orders.Value.Count > 0)
                        order = orders.Value[0];
                }
            }
            return order;
        }

        public async Task<Customer> GetCustomerByOrder(SalesOrder order)
        {
            if (order == null || String.IsNullOrEmpty(order.CustomerNumber))
                return null;
            var query = String.Format("customers?$filter=number eq '{0}'", order.CustomerNumber);

            var apiEndPoint = this.ApiEndPoint + query;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);
                var responseMessage = await httpClient.GetAsync(apiEndPoint);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var customers = JsonConvert.DeserializeObject<Customers>(await responseMessage.Content.ReadAsStringAsync());
                    if (customers != null && customers.Value != null && customers.Value.Count > 0)
                        return customers.Value[0];
                }
            }
            return null;
        }
    }

}
