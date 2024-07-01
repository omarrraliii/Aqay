using Newtonsoft.Json;

namespace aqay_apis.Models.Paymob
{
    public class PaymentKeyResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
