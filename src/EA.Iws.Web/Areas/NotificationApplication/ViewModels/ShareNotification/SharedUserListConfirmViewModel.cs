namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ShareNotification
{
    using Core.Notification;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Views.ShareNotification;

    public class SharedUserListConfirmViewModel
    {
        public Guid NotificationId { get; set; }

        public List<NotificationSharedUser> SharedUsers { get; set; }
        public List<string> SharedUserIds { get; set; }

        public string ConfirmTitle { get; set; }

        public SharedUserListConfirmViewModel()
        {
            ConfirmTitle = ShareNotificationResources.ShareNotificationConfirmTitle;
        }

        public SharedUserListConfirmViewModel(Guid notificationId, List<string> sharedUsers) : this()
        {
            NotificationId = notificationId;
            SharedUserIds = sharedUsers;
        }

        public SharedUserListConfirmViewModel(Guid notificationId, List<NotificationSharedUser> sharedUsers) : this()
        {
            NotificationId = notificationId;
            SharedUsers = sharedUsers;

            SharedUserIds = sharedUsers.Select(p => p.UserId).ToList();
        }
    }
}