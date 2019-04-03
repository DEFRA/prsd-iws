namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.EditEwcCode
{
    using NotificationApplication.ViewModels.WasteCodes;

    public class EditEwcCodeViewModel : BaseWasteCodeViewModel
    {
        public new EditEwcEnterCodesViewModel EnterWasteCodesViewModel
        {
            get { return base.EnterWasteCodesViewModel as EditEwcEnterCodesViewModel; }
            set { base.EnterWasteCodesViewModel = value; }
        }
    }
}