namespace EA.Iws.Web.ViewModels.Movement
{
    using System;
    using Core.Shared;

    public class MovementSummaryViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}