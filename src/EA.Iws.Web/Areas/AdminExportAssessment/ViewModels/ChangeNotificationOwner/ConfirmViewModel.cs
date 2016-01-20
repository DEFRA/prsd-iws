namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.ChangeNotificationOwner
{
    using System;
    using Core.Admin;

    public class ConfirmViewModel
    {
        public ConfirmViewModel()
        {
        }

        public Guid NotificationId { get; set; }

        public ChangeUserData NewUser { get; set; }

        public string NotificationNumber { get; set; }
    }
}