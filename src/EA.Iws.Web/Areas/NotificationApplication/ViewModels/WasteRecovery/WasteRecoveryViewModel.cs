namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;

    public class WasteRecoveryViewModel
    {
        [Required]
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