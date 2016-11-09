namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Delete
{
    using System;

    public class ConfirmViewModel
    {
        public int? Number { get; set; }

        public bool Success { get; set; }

        public Guid NotificationId { get; set; }
    }
}