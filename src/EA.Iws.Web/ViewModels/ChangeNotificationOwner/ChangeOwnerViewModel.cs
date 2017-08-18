namespace EA.Iws.Web.ViewModels.ChangeNotificationOwner
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChangeOwnerViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Enter the e-mail address of who you want to transfer the notification to")]
        public string EmailAddress { get; set; }

        public Guid NotificationId { get; set; }

        public ChangeOwnerViewModel()
        {
        }

        public ChangeOwnerViewModel(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}