using Newtonsoft.Json;

namespace aqay_apis.Models.Paymob
{
    public class AuthTokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
