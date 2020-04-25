using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.businesscentral
{

    public partial class Customers
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("value")]
        public List<Customer> Value { get; set; }
    }

    public partial class Customer
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("taxLiable")]
        public bool TaxLiable { get; set; }

        [JsonProperty("taxAreaId")]
        public Guid TaxAreaId { get; set; }

        [JsonProperty("taxAreaDisplayName")]
        public string TaxAreaDisplayName { get; set; }

        [JsonProperty("taxRegistrationNumber")]
        public string TaxRegistrationNumber { get; set; }

        [JsonProperty("currencyId")]
        public Guid CurrencyId { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("paymentTermsId")]
        public Guid PaymentTermsId { get; set; }

        [JsonProperty("shipmentMethodId")]
        public Guid ShipmentMethodId { get; set; }

        [JsonProperty("paymentMethodId")]
        public Guid PaymentMethodId { get; set; }

        [JsonProperty("blocked")]
        public string Blocked { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }
    }

    public partial class Address
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
