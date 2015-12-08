namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.Complete
{
    using System;
    using Core.Shared;

    public class SuccessViewModel
    {
        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}