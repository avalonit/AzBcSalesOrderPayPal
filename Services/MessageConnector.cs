using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace com.businesscentral
{
    public class MessageConnector
    {
        private ConnectorConfig config;
        private ILogger log;
        public MessageConnector(ConnectorConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
        }
        public MessageResource SendSMS(string text, Customer customer)
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

        public async Task<Response> SendCustomerMail(string messageBody, Customer customer)
        {
            var client = new SendGridClient(config.sendGridApiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress(config.sendGridSender, "Dynamics 365 Business Central"));

            var recipients = new List<EmailAddress>
                { new EmailAddress(customer.Email) };
            msg.AddTos(recipients);

            msg.SetSubject("Pay the order with PayPal");

            //msg.AddContent(MimeType.Text, text);
            msg.AddContent(MimeType.Html, messageBody);
            var response = await client.SendEmailAsync(msg);

            return response;
        }

         public async Task<Response> SendAdminMail(string messageBody)
        {
            var client = new SendGridClient(config.sendGridApiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress(config.sendGridSender, "Dynamics 365 Business Central"));

            var recipients = new List<EmailAddress>
                { new EmailAddress(config.sendGridSender) };
            msg.AddTos(recipients);

            msg.SetSubject("Payment Receipt");

            //msg.AddContent(MimeType.Text, text);
            msg.AddContent(MimeType.Html, messageBody);
            var response = await client.SendEmailAsync(msg);

            return response;
        }

         public async Task<Response> SendTestMail(string messageBody)
        {
            var client = new SendGridClient(config.sendGridApiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress(config.sendGridSender, "Payment WebHook"));

            var recipients = new List<EmailAddress>
                { new EmailAddress(config.sendGridSender) };
            msg.AddTos(recipients);

            msg.SetSubject("Payment WebHook Content");

            //msg.AddContent(MimeType.Text, text);
            msg.AddContent(MimeType.Html, messageBody);
            var response = await client.SendEmailAsync(msg);

            return response;
        }

        
    }
}
