namespace EA.Iws.Web.Areas.Admin.ViewModels.DeleteNotification
{
    using System;

    public class DeleteViewModel
    {
        public Guid? NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public bool IsExportNotification { get; set; }

        public bool Success { get; set; }

        public DeleteViewModel(IndexViewModel model, Guid notificationId)
        {
            NotificationId = notificationId;
            NotificationNumber = model.NotificationNumber;
            IsExportNotification = model.IsExportNotification.GetValueOrDefault();
        }

        public DeleteViewModel()
        {
        }
    }
}