namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    public abstract class BaseWasteCodeViewModel
    {
        public EnterWasteCodesViewModel EnterWasteCodesViewModel { get; set; }

        protected BaseWasteCodeViewModel()
        {
            EnterWasteCodesViewModel = new EnterWasteCodesViewModel();
        }
    }
}