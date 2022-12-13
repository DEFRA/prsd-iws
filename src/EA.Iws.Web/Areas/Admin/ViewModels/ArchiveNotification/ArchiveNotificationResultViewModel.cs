namespace EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.Views.ArchiveNotification;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ArchiveNotificationResultViewModel : IValidatableObject
    {
        public ArchiveNotificationResultViewModel()
        {
        }

        public ArchiveNotificationResultViewModel(UserArchiveNotifications userNotifications)
        {
            NumberOfNotifications = userNotifications.NumberOfNotifications;
            PageNumber = userNotifications.PageNumber;
            PageSize = userNotifications.PageSize;
            Notifications = userNotifications.Notifications;
        }

        public int NumberOfNotifications { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public IList<NotificationArchiveSummaryData> Notifications { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NoNotificationsSelected)
            {
                yield return new ValidationResult(ArchiveNotificationResources.NoNotificationsSelected, new[] { "NoNotificationsSelected" });
            }
        }

        public bool HasAnyResults
        {
            get
            {
                return Notifications.Count > 0;
            }
        }
        
        public bool NoNotificationsSelected
        {
            get
            {
                return Notifications.Where(n => n.IsSelected == true).Count() == 0;
            }
        }
    }
}