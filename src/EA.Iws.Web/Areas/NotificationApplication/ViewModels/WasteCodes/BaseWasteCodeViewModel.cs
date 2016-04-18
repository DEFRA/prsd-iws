namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    public abstract class BaseWasteCodeViewModel
    {
        public virtual EnterWasteCodesViewModel EnterWasteCodesViewModel { get; set; }

        protected BaseWasteCodeViewModel()
        {
            EnterWasteCodesViewModel = new EnterWasteCodesViewModel();
        }
    }
}