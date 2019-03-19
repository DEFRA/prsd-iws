namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.EditEwcCode
{
    using NotificationApplication.ViewModels.WasteCodes;
    using NotificationApplication.Views.EwcCode;

    public class EditEwcEnterCodesViewModel : EnterWasteCodesViewModel
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