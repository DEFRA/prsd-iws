namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.KeyDates
{
    using System.ComponentModel.DataAnnotations;

    public enum KeyDatesCommand
    {
        [Display(Name = "Assessment started")]
        BeginAssessment = 1,

        [Display(Name = "Date completed")]
        NotificationComplete = 2,

        [Display(Name = "Acknowledged on")]
        NotificationAcknowledged = 3,

        [Display(Name = "File closed")]
        FileClosed = 4,

        [Display(Name = "Archive reference")]
        ArchiveReference = 5
    }
}