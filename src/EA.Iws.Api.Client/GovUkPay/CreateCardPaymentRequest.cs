namespace EA.Iws.Api.Client.GovUkPay
{
    using System.Text.Json.Serialization;

    public class CreateCardPaymentRequest
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("return_url")]
        public string ReturnUrl { get; set; }
    }
}