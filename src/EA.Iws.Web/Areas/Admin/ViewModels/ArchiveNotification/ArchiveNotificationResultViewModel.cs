namespace EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification
{
    using DocumentFormat.OpenXml.Wordprocessing;
    using EA.Iws.Core.Admin.ArchiveNotification;
    using EA.Iws.Requests.Notification;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ArchiveNotificationResultViewModel
    {
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

        [Required(ErrorMessage = "Select a notification to archive")]
        public Guid? SelectedNotification { get; set; }

        public bool HasAnyResults
        {
            get 
            {
                return Notifications.Count > 0; 
            }
        }
    }
}