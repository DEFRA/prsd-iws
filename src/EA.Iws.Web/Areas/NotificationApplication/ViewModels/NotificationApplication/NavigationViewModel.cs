namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.Notification;
    using Core.Shared;

    public class NavigationViewModel
    {
        public string NotificationNumber { get; set; }

        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public NotificationApplicationCompletionProgress Progress { get; set; }
    }
}