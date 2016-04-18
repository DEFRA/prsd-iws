namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Views.WasteRecovery;

    public class WasteRecoveryViewModel
    {
        [Required(ErrorMessageResourceName = "SelectionRequired", ErrorMessageResourceType = typeof(WasteRecoveryResources))]
        public ProvidedBy? ProvidedBy { get; set; }

        public WasteRecoveryViewModel()
        {
        }

        public WasteRecoveryViewModel(ProvidedBy? providedBy)
        {
            if (providedBy.HasValue)
            {
                ProvidedBy = providedBy;
            }
        }
    }
}