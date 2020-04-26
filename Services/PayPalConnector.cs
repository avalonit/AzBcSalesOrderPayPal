using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace com.businesscentral
{
    public class PayPalConnector
    {
        private ConnectorConfig config;
        private ILogger log;
        private string ApiOrderEndpoint = string.Empty;
        private string AuthInfo = string.Empty;
        public PayPalConnector(ConnectorConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
            this.ApiOrderEndpoint = String.Format("{0}checkout/orders", config.paypalBaseEndpoint);
            this.AuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", config.payPalClientId, config.paypalSecret)));
        }

        public async Task<PayPalResponseOrder> CreateRequest(SalesOrder order, Customer customer)
        {
            PayPalResponseOrder response = null;
            var paypalOrder = new PayPalOrder()
            {
                Intent = "AUTHORIZE",
                PurchaseUnits = new List<PurchaseUnit>()
                {
                    new PurchaseUnit()
                    {   Amount = new Amount() { CurrencyCode=order.CurrencyCode, Value=order.TotalAmountIncludingTax.ToString() },
                        Payee= new Payee() { EmailAddress=config.payPalSandboxAccount, MerchantId=config.paypalMerchantId },
                    },
                },
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.AuthInfo);

                var jsonObject = JsonConvert.SerializeObject(paypalOrder);
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                var responseMessage = await httpClient.PostAsync(ApiOrderEndpoint, content);

                if (responseMessage.IsSuccessStatusCode)
                    response = JsonConvert.DeserializeObject<PayPalResponseOrder>(await responseMessage.Content.ReadAsStringAsync());
            }
            return response;
        }

    }
}
