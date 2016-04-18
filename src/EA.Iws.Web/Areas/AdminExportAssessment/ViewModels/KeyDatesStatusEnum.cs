namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public enum KeyDatesStatusEnum
    {
        [Display(Name = "Notification received")]
        NotificationReceived = 1,

        [Display(Name = "Payment received")]
        PaymentReceived = 2,

        [Display(Name = "Assessment started")]
        AssessmentCommenced = 3,

        [Display(Name = "Date completed")]
        NotificationComplete = 4,

        [Display(Name = "Transmitted on")]
        NotificationTransmitted = 5,

        [Display(Name = "Acknowledged on")]
        NotificationAcknowledged = 6,

        [Display(Name = "Decision required by")]
        NotificationDecisionDateEntered = 7
    }
}