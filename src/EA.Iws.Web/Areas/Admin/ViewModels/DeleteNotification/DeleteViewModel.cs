namespace EA.Iws.Web.Areas.Admin.ViewModels.DeleteNotification
{
    using System;

    public class DeleteViewModel
    {
        public Guid? NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public bool IsExportNotification { get; set; }
    }
}