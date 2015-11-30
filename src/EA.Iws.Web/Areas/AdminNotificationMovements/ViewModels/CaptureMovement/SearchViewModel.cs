namespace EA.Iws.Web.Areas.AdminNotificationMovements.ViewModels.CaptureMovement
{
    using System.ComponentModel.DataAnnotations;

    public class SearchViewModel
    {
        [Required(ErrorMessageResourceName = "NumberRequired", ErrorMessageResourceType = typeof(SearchViewModelResources))]
        [Display(Name = "Number", ResourceType = typeof(SearchViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "NumberIsInt", ErrorMessageResourceType = typeof(SearchViewModelResources))]
        public int? Number { get; set; }
    }
}