﻿using Newtonsoft.Json;

namespace aqay_apis.Models.Paymob
{
    public class PaymentKeyRequest
    {
        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("expiration")]
        public int Expiration { get; set; }

        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("billing_data")]
        public BillingData BillingData { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("integration_id")]
        public int IntegrationId { get; set; }

        [JsonProperty("lock_order_when_paid")]
        public bool LockOrderWhenPaid { get; set; }
    }
}
