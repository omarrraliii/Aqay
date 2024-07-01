using Newtonsoft.Json;

namespace aqay_apis.Models.Paymob
{
    public class OrderRequest
    {
        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }

        [JsonProperty("delivery_needed")]
        public bool DeliveryNeeded { get; set; } = false;

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; } = 10000;

        [JsonProperty("currency")]
        public string Currency { get; set; } = "EGP";

        [JsonProperty("items")]
        public List<string> Items { get; set; } = new List<string>();
    }
}
