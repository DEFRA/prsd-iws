namespace EA.Iws.Core.ImportNotificationAssessment.FinancialGuarantee
{
    using System.ComponentModel.DataAnnotations;

    public enum ImportFinancialGuaranteeStatus
    {
        [Display(Name = "Awaiting application")]
        AwaitingApplication = 1,
        [Display(Name = "Application received")]
        ApplicationReceived = 2,
        [Display(Name = "Application complete")]
        ApplicationComplete = 3,
        Approved = 4,
        Refused = 5,
        Released = 6
    }
}
