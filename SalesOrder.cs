using System;
using Newtonsoft.Json;

namespace com.businesscentral
{
    public partial class SalesOrder
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("externalDocumentNumber")]
        public string ExternalDocumentNumber { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("postingDate")]
        public DateTime PostingDate { get; set; }

        [JsonProperty("customerId")]
        public Guid CustomerId { get; set; }

        [JsonProperty("contactId")]
        public string ContactId { get; set; }

        [JsonProperty("customerNumber")]
        public long CustomerNumber { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("billToName")]
        public string BillToName { get; set; }

        [JsonProperty("billToCustomerId")]
        public Guid BillToCustomerId { get; set; }

        [JsonProperty("billToCustomerNumber")]
        public long BillToCustomerNumber { get; set; }

        [JsonProperty("shipToName")]
        public string ShipToName { get; set; }

        [JsonProperty("shipToContact")]
        public string ShipToContact { get; set; }

        [JsonProperty("currencyId")]
        public Guid CurrencyId { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("pricesIncludeTax")]
        public bool PricesIncludeTax { get; set; }

        [JsonProperty("paymentTermsId")]
        public Guid PaymentTermsId { get; set; }

        [JsonProperty("shipmentMethodId")]
        public Guid ShipmentMethodId { get; set; }

        [JsonProperty("salesperson")]
        public string Salesperson { get; set; }

        [JsonProperty("partialShipping")]
        public bool PartialShipping { get; set; }

        [JsonProperty("requestedDeliveryDate")]
        public DateTime RequestedDeliveryDate { get; set; }

        [JsonProperty("discountAmount")]
        public long DiscountAmount { get; set; }

        [JsonProperty("discountAppliedBeforeTax")]
        public bool DiscountAppliedBeforeTax { get; set; }

        [JsonProperty("totalAmountExcludingTax")]
        public double TotalAmountExcludingTax { get; set; }

        [JsonProperty("totalTaxAmount")]
        public double TotalTaxAmount { get; set; }

        [JsonProperty("totalAmountIncludingTax")]
        public double TotalAmountIncludingTax { get; set; }

        [JsonProperty("fullyShipped")]
        public bool FullyShipped { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("sellingPostalAddress")]
        public IngPostalAddress SellingPostalAddress { get; set; }

        [JsonProperty("billingPostalAddress")]
        public IngPostalAddress BillingPostalAddress { get; set; }

        [JsonProperty("shippingPostalAddress")]
        public IngPostalAddress ShippingPostalAddress { get; set; }
    }

    public partial class IngPostalAddress
    {
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("countryLetterCode")]
        public string CountryLetterCode { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
    }


}
