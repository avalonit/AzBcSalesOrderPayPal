using System;
using System.Text;

namespace com.businesscentral
{

    public partial class MessageComposer
    {
        public string DataBindMessage(SalesOrder order)
        {
            StringBuilder message = new StringBuilder();

            if (order == null)
                return String.Format("Sorry, order not found");

            message.Append(String.Format("The {0} order ", order.Number));
            message.Append(String.Format("of customer {0}, ", order.CustomerName));
            message.Append(String.Format("is in status {0}, ", order.Status));

            if (order.ShippingPostalAddress != null)
            {
                message.Append(String.Format("shipping to {0}", order.ShipToName));
                if (!String.IsNullOrEmpty(order.ShippingPostalAddress.Street))
                    message.Append(String.Format(" ", order.ShippingPostalAddress.Street));
                if (!String.IsNullOrEmpty(order.ShippingPostalAddress.City))
                    message.Append(String.Format(" ", order.ShippingPostalAddress.City));
                if (!String.IsNullOrEmpty(order.ShippingPostalAddress.State))
                    message.Append(String.Format(" ", order.ShippingPostalAddress.State));
                if (!String.IsNullOrEmpty(order.ShippingPostalAddress.CountryLetterCode))
                    message.Append(String.Format(" ", order.ShippingPostalAddress.CountryLetterCode));
            }

            if (order.LastModifiedDateTime.Year != 1)
                message.Append(String.Format(", last updated {0}.", order.LastModifiedDateTime.ToString("dd/MM/yyyy HH:mm")));

            message.Append(String.Format(". Please reply YES to this message if you want an order confirmation is sent to your customer."));

            return message.ToString();
        }
    }
}
