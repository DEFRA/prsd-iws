namespace EA.Iws.Web.Areas.Reports.ViewModels.MissingShipments
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            From = new OptionalDateInputViewModel(true);
            To = new OptionalDateInputViewModel(true);
        }

        [Display(Name = "From", ResourceType = typeof(ExportStats.IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "FromRequired",
            ErrorMessageResourceType = typeof(ExportStats.IndexViewModelResources))]
        public OptionalDateInputViewModel From { get; set; }

        [Display(Name = "To", ResourceType = typeof(ExportStats.IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ToRequired",
            ErrorMessageResourceType = typeof(ExportStats.IndexViewModelResources))]
        public OptionalDateInputViewModel To { get; set; }
    }
}