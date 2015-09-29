namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.RecoveryInfo
{
    using Core.Shared;

    public class RecoveryInfoViewModel
    {
        public ProvidedBy? ProvidedBy { get; set; }

        public RecoveryInfoViewModel()
        {
        }

        public RecoveryInfoViewModel(ProvidedBy? providedBy)
        {
            if (providedBy.HasValue)
            {
                ProvidedBy = providedBy;
            }
        }
    }
}