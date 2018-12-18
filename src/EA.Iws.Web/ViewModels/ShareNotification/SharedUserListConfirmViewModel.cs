namespace EA.Iws.Web.ViewModels.ShareNotification
{
    using Core.Notification;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Views.ShareNotification;

    public class SharedUserListConfirmViewModel
    {
        public Guid NotificationId { get; set; }

        public List<NotificationSharedUser> SharedUsers { get; set; } = new List<NotificationSharedUser>();
        public List<string> SharedUserIds { get; set; }

        public string ConfirmTitle { get; set; }

        public SharedUserListConfirmViewModel()
        {
            this.ConfirmTitle = ShareNotificationResources.ShareNotificationConfirmTitle;
        }

        public SharedUserListConfirmViewModel(Guid notificationId, List<string> sharedUsers) : this()
        {
            this.NotificationId = notificationId;
            this.SharedUserIds = sharedUsers;
        }

        public SharedUserListConfirmViewModel(Guid notificationId, List<NotificationSharedUser> sharedUsers) : this()
        {
            this.NotificationId = notificationId;
            this.SharedUsers = sharedUsers.ToList();

            this.SharedUserIds = sharedUsers.Select(p => p.UserId).ToList();
        }
    }
}