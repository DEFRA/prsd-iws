namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System.ComponentModel.DataAnnotations;
    using Views.WasteRecovery;

    public class PercentageViewModel
    {
        [Required(ErrorMessageResourceName = "ValueRequired", ErrorMessageResourceType = typeof(PercentageResources))]
        [Range(0, 100, ErrorMessageResourceName = "PercentageRange", ErrorMessageResourceType = typeof(PercentageResources))]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceName = "PercentageRecoverableValid", ErrorMessageResourceType = typeof(PercentageResources))]
        [Display(Name = "PercentageRecoverable", ResourceType = typeof(PercentageResources))]
        public decimal? PercentageRecoverable { get; set; }
    }
}