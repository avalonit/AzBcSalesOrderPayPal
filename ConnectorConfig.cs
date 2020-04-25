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
        

    }
}
