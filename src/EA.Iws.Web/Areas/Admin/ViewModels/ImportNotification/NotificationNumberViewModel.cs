namespace EA.Iws.Web.Areas.Admin.ViewModels.ImportNotification
{
    using System.ComponentModel.DataAnnotations;

    public class NotificationNumberViewModel
    {
        private string notificationNumber;

        [Required]
        [StringLength(25, ErrorMessage = "Import notification number cannot be longer than 25 characters")]
        [Display(Name = "Notification number")]
        public string NotificationNumber
        {
            get { return notificationNumber; }
            set
            {
                if (value == null)
                {
                    notificationNumber = null;
                    return;
                }
                notificationNumber = value.Trim();
            }
        }
    }
}