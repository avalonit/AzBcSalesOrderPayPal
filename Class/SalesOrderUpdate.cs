using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace com.businesscentral
{

    public partial class SalesOrderUpdate
    {
        [JsonProperty("paymentTermsId")]
        public Guid PaymentTermsId { get; set; }

        [JsonProperty("externalDocumentNumber")]
        public string ExternalDocumentNumber { get; set; }

        
    }
}
