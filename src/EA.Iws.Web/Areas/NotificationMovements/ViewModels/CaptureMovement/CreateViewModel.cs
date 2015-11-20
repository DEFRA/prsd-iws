namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.CaptureMovement
{
    using System.ComponentModel.DataAnnotations;
    using Web.ViewModels.Shared;

    public class CreateViewModel
    {
        public int Number { get; set; }

        [Display(Name = "PrenotificationDateLabel", ResourceType = typeof(CreateViewModelResources))]
        public OptionalDateInputViewModel PrenotificationDate { get; set; }

        [Display(Name = "ActualDateLabel", ResourceType = typeof(CreateViewModelResources))]
        public OptionalDateInputViewModel ActualShipmentDate { get; set; }

        public CreateViewModel()
        {
            PrenotificationDate = new OptionalDateInputViewModel();
            ActualShipmentDate = new OptionalDateInputViewModel();
        }
    }
}