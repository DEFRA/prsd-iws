namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class NotificationSwitcherViewModel
    {
        [Required]
        public string OriginalNumber { get; set; }

        [Required]
        public string Number { get; set; }

        public NotificationSwitcherViewModel()
        {
        }

        public NotificationSwitcherViewModel(string number)
        {
            OriginalNumber = number;
            Number = OriginalNumber;
        }
    }
}