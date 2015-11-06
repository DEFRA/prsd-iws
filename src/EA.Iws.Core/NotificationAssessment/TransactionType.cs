namespace EA.Iws.Core.NotificationAssessment
{
    using System.ComponentModel.DataAnnotations;

    public enum TransactionType
    {
        [Display(Name = "Payment")]
        Payment = 1,

        [Display(Name = "Refund")]
        Refund = 2
    }
}
