namespace EA.Iws.Web.ViewModels.ShareNotification
{
    using Core.Notification;
    using Infrastructure.Validation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class ShareNotificationViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public Guid NotificationId { get; set; }

        public SelectList SharedUsersList { get; set; }

        [NotificationShareUserValidation(5, ErrorMessage = "Maxiumum of 5 email addresses")]
        public List<NotificationSharedUser> SelectedSharedUsers { get; set; } 

        public ShareNotificationViewModel()
        {
            this.SelectedSharedUsers = new List<NotificationSharedUser>();
        }

        public ShareNotificationViewModel(Guid notificationId) : this()
        {
            NotificationId = notificationId;
        }

        public ShareNotificationViewModel(Guid notificationId, List<NotificationSharedUser> sharedUsers) : this()
        {
            if (sharedUsers != null)
            {
                this.SelectedSharedUsers = sharedUsers;
            }
            NotificationId = notificationId;
        }

        public void SetSharedUsers(IEnumerable<NotificationSharedUser> sharedUsers)
        {
            SharedUsersList = new SelectList(sharedUsers.Select(c => new SelectListItem()
            {
                Text = c.Email,
                Value = c.UserId.ToString()
            }), "Value", "Text");
        }
    }
}