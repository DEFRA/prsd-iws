namespace EA.Iws.Api.Client.GovUkPay
{
    public class CreatePaymentResult
    {
        public string PaymentId { get; set; }

        public string NextUrl { get; set; }

        public PaymentState State { get; set; }
    }
}