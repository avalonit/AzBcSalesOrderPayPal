using System;
using Microsoft.Extensions.Configuration;

namespace com.businesscentral
{

    public partial class ConnectorConfig
    {
        public ConnectorConfig(IConfigurationRoot config)
        {
            if (config != null)
            {
                tenant = config["tenant"];
                companyID = config["companyID"];
                apiVersion1 = config["apiVersion1"];
                apiVersion2 = config["apiVersion2"];
                authInfo = config["authInfo"];
                twilioSID = config["twilioSID"];
                twilioToken = config["twilioToken"];
                twilioFromNumber = config["twilioFromNumber"];
                twilioDefaultNumber = config["twilioDefaultNumber"];
                sendGridApiKey = config["sendGridApiKey"];
                sendGridSender = config["sendGridSender"];
                payPalSandboxAccount = config["payPalSandboxAccount"];
                payPalClientId = config["payPalClientId"];
                paypalSecret = config["paypalSecret"];
                paypalMerchantId = config["paypalMerchantId"];
                paypalBaseEndpoint = config["paypalBaseEndpoint"];
                paypalPlaceholderUrl = config["paypalPlaceholderUrl"];
                paymentMethodsIdPayPal = config["paymentMethodsIdPayPal"];
                paymentMethodsIdPayPalPaid = config["paymentMethodsIdPayPalPaid"];
                paymentTermsIdPayPal = config["paymentTermsIdPayPal"];
                paymentTermsIdPayPalPaid = config["paymentTermsIdPayPalPaid"];

            }
            // If you are customizing here it means you
            //  should give a look on how use
            //  azure configuration file
            if (String.IsNullOrEmpty(tenant))
                tenant = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxdxxxxxxx/Sandbox";
            if (String.IsNullOrEmpty(companyID))
                companyID = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            if (String.IsNullOrEmpty(apiVersion1))
                apiVersion1 = "v2.0";
            if (String.IsNullOrEmpty(apiVersion2))
                apiVersion2 = "v1.0";
            if (String.IsNullOrEmpty(authInfo))
                authInfo = "your_username:yout_web_service_access_key";

            if (String.IsNullOrEmpty(twilioSID))
                twilioSID = "your SID from https://www.twilio.com/console";
            if (String.IsNullOrEmpty(twilioToken))
                twilioToken = "your Token from https://www.twilio.com/console";
            if (String.IsNullOrEmpty(twilioFromNumber))
                twilioFromNumber = "your Twilio number from https://www.twilio.com/console";
            if (String.IsNullOrEmpty(twilioDefaultNumber))
                twilioDefaultNumber = "your Twilio number from https://www.twilio.com/console";

            if (String.IsNullOrEmpty(sendGridApiKey))
                sendGridApiKey = "your API key from https://app.sendgrid.com/settings/api_keys";
            if (String.IsNullOrEmpty(sendGridSender))
                sendGridSender = "your email";

            if (String.IsNullOrEmpty(payPalSandboxAccount))
                payPalSandboxAccount = "your Sandbox Account from https://developer.paypal.com/developer/applications";
            if (String.IsNullOrEmpty(payPalClientId))
                payPalClientId = "your Client ID from https://developer.paypal.com/developer/applications";
            if (String.IsNullOrEmpty(paypalSecret))
                paypalSecret = "your Secret Key from https://developer.paypal.com/developer/applications";
            if (String.IsNullOrEmpty(paypalMerchantId))
                paypalMerchantId = "your merchant id";
            if (String.IsNullOrEmpty(paypalBaseEndpoint))
                paypalBaseEndpoint = "https://api.sandbox.paypal.com/v2/";
            if (String.IsNullOrEmpty(paypalPlaceholderUrl))
                paypalPlaceholderUrl = "http://{this_function_public_url}.azurewebsites.net/api/";

            if (String.IsNullOrEmpty(paymentMethodsIdPayPal))
                paymentMethodsIdPayPal = "{payment method id for PAYPAL in business central table}";
            if (String.IsNullOrEmpty(paymentMethodsIdPayPalPaid))
                paymentMethodsIdPayPalPaid = "{payment method id for PAYPAL_OK in business central table}";

            if (String.IsNullOrEmpty(paymentTermsIdPayPal))
                paymentTermsIdPayPal = "{payment term id for PAYPAL in business central table}";
            if (String.IsNullOrEmpty(paymentTermsIdPayPalPaid))
                paymentTermsIdPayPalPaid = "{payment term id for PAYPAL_OK in business central table}";

        }

        public String tenant;
        public String companyID;
        public String apiVersion1;
        public String apiVersion2;
        public String authInfo;
        public String twilioSID;
        public String twilioToken;
        public String twilioFromNumber;
        public String twilioDefaultNumber;
        public String sendGridApiKey;
        public String sendGridSender;
        public String payPalSandboxAccount;
        public String payPalClientId;
        public String paypalSecret;
        public String paypalMerchantId;
        public String paypalBaseEndpoint;
        public String paypalPlaceholderUrl;
        public String paymentMethodsIdPayPal;
        public String paymentMethodsIdPayPalPaid;
        public String paymentTermsIdPayPal;
        public String paymentTermsIdPayPalPaid;


    }
}
