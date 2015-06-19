namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Requests.Shared;

    public class NotificationOverviewViewModel
    {
        public string NotificationNumber { get; set; }

        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}