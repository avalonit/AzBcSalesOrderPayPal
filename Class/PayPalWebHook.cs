using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace com.businesscentral
{

    public partial class PayPalResponseOrder
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class PayPalWebHook
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("event_version")]
        public string EventVersion { get; set; }

        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("resource_version")]
        public string ResourceVersion { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("resource")]
        public Resource Resource { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }
    }

    public partial class Link
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

    public partial class Resource
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("final_capture")]
        public bool FinalCapture { get; set; }

        [JsonProperty("seller_protection")]
        public SellerProtection SellerProtection { get; set; }

        [JsonProperty("seller_receivable_breakdown")]
        public SellerReceivableBreakdown SellerReceivableBreakdown { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("update_time")]
        public DateTime UpdateTime { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }
    }

    public partial class Amount
    {
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class SellerProtection
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("dispute_categories")]
        public List<string> DisputeCategories { get; set; }
    }

    public partial class SellerReceivableBreakdown
    {
        [JsonProperty("gross_amount")]
        public Amount GrossAmount { get; set; }

        [JsonProperty("paypal_fee")]
        public Amount PaypalFee { get; set; }

        [JsonProperty("net_amount")]
        public Amount NetAmount { get; set; }
    }
}
