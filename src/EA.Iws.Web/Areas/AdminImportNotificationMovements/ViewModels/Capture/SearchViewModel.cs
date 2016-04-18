namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Capture
{
    using System.ComponentModel.DataAnnotations;

    public class SearchViewModel
    {
        [Required(ErrorMessageResourceType = typeof(SearchViewModelResources), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(SearchViewModelResources), ErrorMessageResourceName = "Range")]
        [Display(Name = "Label", ResourceType = typeof(SearchViewModelResources))]
        public int? Number { get; set; }
    }
}