namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System.ComponentModel.DataAnnotations;

    public enum ImportNotificationStatus
    {
        New = 1,
        [Display(Name = "Notification received")]
        NotificationReceived = 2,
        Submitted = 3,
        [Display(Name = "Awaiting payment")]
        AwaitingPayment = 4,
        [Display(Name = "Awaiting assessment")]
        AwaitingAssessment = 5,
        [Display(Name = "In assessment")]
        InAssessment = 6,
        [Display(Name = "Ready to acknowledge")]
        ReadyToAcknowledge = 7,
        [Display(Name = "Decision required by")]
        DecisionRequiredBy = 8,
        [Display(Name = "Consented")]
        Consented = 9,
        [Display(Name = "Consent withdrawn")]
        ConsentWithdrawn = 10,
        [Display(Name = "Objected")]
        Objected = 11,
        [Display(Name = "Withdrawn")]
        Withdrawn = 12
    }
}
