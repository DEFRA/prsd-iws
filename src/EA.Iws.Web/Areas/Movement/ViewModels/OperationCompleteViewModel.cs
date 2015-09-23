namespace EA.Iws.Web.Areas.Movement.ViewModels
{
    using System;
    using Core.Shared;

    public class OperationCompleteViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}