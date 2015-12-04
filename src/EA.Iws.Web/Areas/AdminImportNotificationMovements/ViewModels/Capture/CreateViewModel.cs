namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Capture
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class CreateViewModel
    {
        public int Number { get; set; }

        [RequiredDateInput(ErrorMessageResourceName = "ActualShipmentDateRequired", ErrorMessageResourceType = typeof(CreateViewModelResources))]
        [Display(Name = "ActualShipmentDate", ResourceType = typeof(CreateViewModelResources))]
        public OptionalDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "PrenotificationDate", ResourceType = typeof(CreateViewModelResources))]
        public OptionalDateInputViewModel PrenotificationDate { get; set; }

        public CreateViewModel()
        {
            ActualShipmentDate = new OptionalDateInputViewModel(true);
            PrenotificationDate = new OptionalDateInputViewModel(true);
        }
    }
}