using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<SalesOrder> GetOrderByWebhook(WebHookEvent ev)
        {
            SalesOrder orders = null;

            if (ev == null || ev.Value == null || ev.Value.Count == 0)
                return null;
            if (!ev.Value[0].Resource.Contains("salesOrders"))
                return null;

            var apiEndPoint = this.ApiWebHookEndPoint + ev.Value[0].Resource;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);
                var responseMessage = await httpClient.GetAsync(apiEndPoint);
                if (responseMessage.IsSuccessStatusCode)
                    orders = JsonConvert.DeserializeObject<SalesOrder>(await responseMessage.Content.ReadAsStringAsync());
            }
            return orders;
        }

        public async Task<Employees> GetSaleagentByOrder(SalesOrder order)
        {
            Employees employees = null;

            if (order == null || String.IsNullOrEmpty(order.Salesperson))
                return null;
            var query = String.Format("employees?$filter=number eq '{0}'", order.Salesperson);

            var apiEndPoint = this.ApiEndPoint + query;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);
                var responseMessage = await httpClient.GetAsync(apiEndPoint);
                if (responseMessage.IsSuccessStatusCode)
                    employees = JsonConvert.DeserializeObject<Employees>(await responseMessage.Content.ReadAsStringAsync());
            }
            return employees;
        }
    }

}
