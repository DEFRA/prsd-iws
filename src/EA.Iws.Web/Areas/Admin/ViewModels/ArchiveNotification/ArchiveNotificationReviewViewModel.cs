namespace EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.Views.ArchiveNotification;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ArchiveNotificationReviewViewModel : IValidatableObject
    {
        public ArchiveNotificationReviewViewModel()
        {
        }

        public IList<NotificationArchiveSummaryData> SelectedNotifications { get; set; }

        public bool HasAnyResults { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HasAnyResults == false)
            {
                yield return new ValidationResult(ArchiveNotificationResources.NoNotificationSelected, new[] { "HasAnyResults" });
            }
        }
    }
}