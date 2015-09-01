namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.EwcCode
{
    using WasteCodes;

    public class EwcEnterCodesViewModel : EnterWasteCodesViewModel
    {
        public override string ValidationMessage
        {
            get { return "Please enter a code"; }
        }

        public override bool IsNotApplicable
        {
            get { return false; }
        }
    }
}