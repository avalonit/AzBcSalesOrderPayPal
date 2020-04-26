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

    public partial class MessageComposer
    {

        private ConnectorConfig config;
        private ILogger log;

        public MessageComposer(ConnectorConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
        }
        public string DataBindMessage(SalesOrder order)
        {
            StringBuilder message = new StringBuilder();

            if (order == null)
                return String.Format("Sorry, order not found");

            message.Append(String.Format("The {0} order ", order.Number));
            message.Append(String.Format("is in status {0} and ready to be paid with PayPal, ", order.Status));
            message.Append(String.Format("the total amount to pay is {0} {1}.", order.TotalAmountIncludingTax.ToString(), order.CurrencyCode));

            return message.ToString();
        }

        public string DataBindEmail(SalesOrder order, PayPalResponseOrder responseOrder)
        {
            StringBuilder message = new StringBuilder();

            if (order == null)
                return String.Format("Sorry, order not found");

            message.Append(String.Format("The {0} order ", order.Number));
            message.Append(String.Format("is in status {0} and ready to be paid with PayPal<br>", order.Status));
            message.Append(String.Format("The total amount to pay is {0} {1}<br>", order.TotalAmountIncludingTax.ToString(), order.CurrencyCode));
            message.Append(String.Format("<a href='{0}/AzPayPal?orderID={1}'>Click here to pay with paypal</a>", config.paypalPlaceholderUrl, responseOrder.Id));

            return message.ToString();
        }

         public string DataBindEmailPayment(SalesOrder order)
        {
            StringBuilder message = new StringBuilder();

            if (order == null)
                return String.Format("Sorry, order not found");

            message.Append(String.Format("The {0} order ", order.Number));
            message.Append(String.Format("is in status {0} was successfully paid with PayPal<br>", order.Status));
            message.Append(String.Format("The total amount to pay is {0} {1}<br>", order.TotalAmountIncludingTax.ToString(), order.CurrencyCode));

            return message.ToString();
        }

        public string DataBindPayPalPage(string orderId)
        {
            StringBuilder message = new StringBuilder();

            message.Append(String.Format("<body>"));
            message.Append(String.Format("<script src=\"https://www.paypal.com/sdk/js?client-id={0}\"  data-order-id=\"{1}\">", config.payPalClientId, orderId));
            message.Append(String.Format("</script>"));
            message.Append(String.Format("<div id=\"paypal-button-container\"></div>"));
            message.Append(String.Format("<script>"));
            message.Append(String.Format("paypal.Buttons().render('#paypal-button-container');"));
            message.Append(String.Format("</script>"));
            message.Append(String.Format("</body>"));

            return message.ToString();
        }
    }
}
