using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.businesscentral
{
    public partial class WebHookEvent
    {
        [JsonProperty("value")]
        public List<Value> Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("subscriptionId")]
        public string SubscriptionId { get; set; }

        [JsonProperty("clientState")]
        public string ClientState { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("changeType")]
        public string ChangeType { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }
    }

}
