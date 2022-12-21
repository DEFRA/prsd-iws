namespace EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.Views.ArchiveNotification;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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

        public IList<NotificationArchiveSummaryData> SelectedNotifications { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HasAnyNotificationSelected == false)
            {
                yield return new ValidationResult(ArchiveNotificationResources.NoNotificationSelected, new[] { "HasAnyNotificationSelected" });
            }
        }

        public bool HasAnyResults
        {
            get
            {
                return Notifications.Count > 0;
            }
        }

        private bool hasAnyNotificationSelected = false;
        public bool HasAnyNotificationSelected
        {
            get
            {
                return hasAnyNotificationSelected;
            }
            set
            {
                hasAnyNotificationSelected = value;
            }
        }
        public bool HasNotificationsSelectedMoreThanOne { get; set; }

        public int NumberOfNotificationsSelected { get; set; }

        [Display(Name = "Select All")]
        public bool IsSelectAllChecked { get; set; }
    }
}