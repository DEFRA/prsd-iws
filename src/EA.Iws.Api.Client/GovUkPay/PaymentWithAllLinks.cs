namespace EA.Iws.Api.Client.GovUkPay
{
    using System.Text.Json.Serialization;

    public class PaymentWithAllLinks
    {
        [JsonPropertyName("payment_id")]
        public string PaymentId { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("state")]
        public PaymentState State { get; set; }

        [JsonPropertyName("_links")]
        public PaymentLinks Links { get; set; }
    }
}