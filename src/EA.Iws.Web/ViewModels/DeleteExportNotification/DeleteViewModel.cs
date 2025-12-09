namespace EA.Iws.Web.ViewModels.DeleteExportNotification
{
    using System;

    public class DeleteViewModel
    {
        public Guid? NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public bool Success { get; set; }

        public DeleteViewModel(IndexViewModel model, Guid notificationId)
        {
            NotificationId = notificationId;
            NotificationNumber = model.NotificationNumber;
        }

        public DeleteViewModel()
        {
        }
    }
}