using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace com.businesscentral
{
    public class BusinessCentralConnector
    {
        private ConnectorConfig config;
        private string ApiWebHookEndPoint = string.Empty;
        private string ApiEndPoint = string.Empty;
        private string AuthInfo = string.Empty;
        public BusinessCentralConnector(ConnectorConfig config)
        {
            this.config = config;
            this.ApiWebHookEndPoint = String.Format("https://api.businesscentral.dynamics.com/{0}/{1}/", config.apiVersion1, config.tenant);
            this.ApiEndPoint = String.Format("https://api.businesscentral.dynamics.com/{0}/{1}/api/{2}/companies({3})/",
                                    config.apiVersion1, config.tenant, config.apiVersion2, config.companyID);

            this.AuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(config.authInfo));
        }
        public async Task<SalesOrder> GetPayPalOrderByWebhook(WebHookEvent ev)
        {
            var apiEndPoint = this.ApiWebHookEndPoint + ev.Resource;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);
                var responseMessage = await httpClient.GetAsync(apiEndPoint);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var order = JsonConvert.DeserializeObject<SalesOrder>(await responseMessage.Content.ReadAsStringAsync());
                    if (order != null)
                        return order.PaymentTermsId.Equals("PAYPAL") ? order : null;
                }
            }
            return null;
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
