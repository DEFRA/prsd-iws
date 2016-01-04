namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.KeyDates
{
    using System.ComponentModel.DataAnnotations;

    public enum KeyDatesCommand
    {
        [Display(Name = "Notification received")]
        NotificationReceived = 1,

        [Display(Name = "Assessment started")]
        BeginAssessment = 2,

        [Display(Name = "Date completed")]
        NotificationComplete = 4,

        [Display(Name = "Acknowledged on")]
        NotificationAcknowledged = 5
    }
}