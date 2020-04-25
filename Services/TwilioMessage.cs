using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace com.businesscentral
{
    public class TwilioConnector
    {
        public MessageResource SendMessage(string text, Customer customer, ConnectorConfig config)
        {
            // Find your Account Sid and Token at twilio.com/console
            var accountSid = config.twilioSID;
            var authToken = config.twilioToken;

            var toNumber = customer == null || String.IsNullOrEmpty(customer.PhoneNumber) ? config.twilioDefaultNumber : customer.PhoneNumber;

            if (!String.IsNullOrEmpty(toNumber))
            {
                TwilioClient.Init(accountSid, authToken);

                // Twilio allow sending messages only to verified numbers
                //   Number verification procedure available at this link:
                //   twilio.com/user/account/phone-numbers/verified
                var message = MessageResource.Create(
                    body: text,
                    from: new Twilio.Types.PhoneNumber(config.twilioFromNumber),
                    to: new Twilio.Types.PhoneNumber(toNumber)
                );

                return message;

            }
            return null;
        }
    }
}
