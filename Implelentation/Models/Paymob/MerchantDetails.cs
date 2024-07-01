using Newtonsoft.Json;

namespace aqay_apis.Models.Paymob
{
    public class MerchantDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("phones")]
        public List<string> Phones { get; set; }

        [JsonProperty("company_emails")]
        public List<string> CompanyEmails { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }
    }
}
