namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.EwcCode
{
    using Views.EwcCode;
    using WasteCodes;

    public class EwcEnterCodesViewModel : EnterWasteCodesViewModel
    {
        public override string ValidationMessage
        {
            get { return EwcCodeResources.CodeRequired; }
        }

        public override bool IsNotApplicable
        {
            get { return false; }
        }
    }
}