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
        InAssessment = 6
    }
}
