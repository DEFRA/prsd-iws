namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.BulkUploadTemplate
{
    using System;
    using Core.Shared;

    public class BulkUploadTemplateViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}