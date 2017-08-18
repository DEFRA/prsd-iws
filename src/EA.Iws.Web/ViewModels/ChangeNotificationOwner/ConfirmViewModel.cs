namespace EA.Iws.Web.ViewModels.ChangeNotificationOwner
{
    using System;

    [Serializable]
    public class ConfirmViewModel
    {
        public string NotificationNumber { get; set; }

        public string EmailAddress { get; set; }

        public Guid NotificationId { get; set; }
    }
}