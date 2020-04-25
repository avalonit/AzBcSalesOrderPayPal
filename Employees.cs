using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.businesscentral
{

    public partial class Employees
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("value")]
        public List<Employee> Value { get; set; }
    }

    public partial class Employee
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("personalEmail")]
        public string PersonalEmail { get; set; }

        [JsonProperty("employmentDate")]
        public DateTime EmploymentDate { get; set; }

        [JsonProperty("terminationDate")]
        public DateTime TerminationDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("statisticsGroupCode")]
        public string StatisticsGroupCode { get; set; }

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
        public long PostalCode { get; set; }
    }

}
