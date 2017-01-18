namespace EA.Iws.Web.ViewModels.Applicant
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Prsd.Core.Helpers;
    using Requests.Notification;

    public class UserNotificationsViewModel
    {
        public UserNotificationsViewModel()
        {
        }

        public UserNotificationsViewModel(UserNotifications userNotifications)
        {
            NumberOfNotifications = userNotifications.NumberOfNotifications;
            PageNumber = userNotifications.PageNumber;
            PageSize = userNotifications.PageSize;
            Notifications = userNotifications.Notifications;
        }

        public int NumberOfNotifications { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public IList<NotificationApplicationSummaryData> Notifications { get; set; }

        public SelectList NotificationStatuses
        {
            get
            {
                var units = Enum.GetValues(typeof(NotificationStatus))
                    .Cast<NotificationStatus>()
                    .Where(s => s != NotificationStatus.InDetermination)
                    .Select(s => new SelectListItem
                    {
                        Text = EnumHelper.GetDisplayName(s),
                        Value = ((int)s).ToString()
                    })
                    .OrderBy(s => s.Text)
                    .ToList();

                units.Insert(0, new SelectListItem { Text = "View all", Value = string.Empty });

                return new SelectList(units, "Value", "Text", SelectedNotificationStatus);
            }
        }

        public NotificationStatus? SelectedNotificationStatus { get; set; }
    }
}