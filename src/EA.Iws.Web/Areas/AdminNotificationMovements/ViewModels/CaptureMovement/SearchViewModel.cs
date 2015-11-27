namespace EA.Iws.Web.Areas.AdminNotificationMovements.ViewModels.CaptureMovement
{
    using System.ComponentModel.DataAnnotations;

    public class SearchViewModel
    {
        [Display(Name = "Number", ResourceType = typeof(SearchViewModelResources))]
        public int Number { get; set; }
    }
}