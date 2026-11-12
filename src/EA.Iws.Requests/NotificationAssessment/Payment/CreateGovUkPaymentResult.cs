namespace EA.Iws.Requests.NotificationAssessment.Payment
{
    public class CreateGovUkPaymentResult
    {
        public bool AlreadyPaid { get; set; }

        public bool PaymentInProgress { get; set; }

        public string NotificationNumber { get; set; }

        public string SecureToken { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string NextUrl { get; set; }
    }
}