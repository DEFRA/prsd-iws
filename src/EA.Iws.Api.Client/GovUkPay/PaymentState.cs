namespace EA.Iws.Api.Client.GovUkPay
{
    using System.Text.Json.Serialization;

    public class PaymentState
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("finished")]
        public bool Finished { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}