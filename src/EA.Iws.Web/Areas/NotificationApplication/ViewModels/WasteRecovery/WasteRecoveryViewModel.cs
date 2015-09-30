namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using Core.Shared;

    public class WasteRecoveryViewModel
    {
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