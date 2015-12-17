namespace EA.Iws.Web.Areas.Admin.ViewModels.ImportNotification
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class NotificationReceivedDateViewModel
    {
        public string NotificationNumber { get; set; }

        [Display(Name = "ReceivedDate", ResourceType = typeof(NotificationReceivedDateViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ReceivedDateRequired", ErrorMessageResourceType = typeof(NotificationReceivedDateViewModelResources))]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        public NotificationReceivedDateViewModel()
        {
            ReceivedDate = new OptionalDateInputViewModel(true);
        }
    }
}