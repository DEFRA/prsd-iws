namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.EwcCode
{
    using WasteCodes;

    public class EwcCodeViewModel : BaseWasteCodeViewModel
    {
        public new EwcEnterCodesViewModel EnterWasteCodesViewModel
        {
            get { return base.EnterWasteCodesViewModel as EwcEnterCodesViewModel; }
            set { base.EnterWasteCodesViewModel = value; }
        }
    }
}