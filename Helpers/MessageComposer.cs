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
            message.Append(String.Format("is in status {0} and ready to be paid with PayPal, ", order.Status));
            message.Append(String.Format("the total amount to pay is {0}.", order.TotalAmountIncludingTax.ToString()));

            return message.ToString();
        }
    }
}
