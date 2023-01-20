namespace EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.Views.ArchiveNotification;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ArchiveNotificationArchivedViewModel : IValidatableObject
    {
        public ArchiveNotificationArchivedViewModel()
        {
        }

        public IList<NotificationArchiveSummaryData> ArchivedNotifications { get; set; }

        public int SuccessCount { get; set; }

        public int FailureCount { get; set; }

        public bool HasAnyNotificationFailures { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult(ArchiveNotificationResources.FailureMsg, new[] { "HasAnyNotificationFailures" });
        }
    }
}