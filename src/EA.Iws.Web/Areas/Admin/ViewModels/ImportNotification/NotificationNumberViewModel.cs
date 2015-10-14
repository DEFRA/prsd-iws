namespace EA.Iws.Web.Areas.Admin.ViewModels.ImportNotification
{
    using System.ComponentModel.DataAnnotations;

    public class NotificationNumberViewModel
    {
        private string notificationNumber;

        [Required]
        [StringLength(14, ErrorMessage = "Import notification number cannot be longer than 14 characters")]
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
                notificationNumber = value.Trim().Replace(" ", string.Empty);
            }
        }
    }
}