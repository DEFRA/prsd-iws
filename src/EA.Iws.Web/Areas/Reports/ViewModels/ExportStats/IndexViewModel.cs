namespace EA.Iws.Web.Areas.Reports.ViewModels.ExportStats
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

        [Display(Name = "From", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "FromRequired",
            ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel From { get; set; }

        [Display(Name = "To", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ToRequired",
            ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel To { get; set; }
    }
}