namespace EA.Iws.Web.Areas.Admin.ViewModels.Worklist
{
    using Core.ImportNotificationAssessment;

    public class ImportWorklistFilterViewModel
    {
        public string NotificationNumber { get; set; }
        public string Officer { get; set; }
        public ImportNotificationStatus[] SelectedStatuses { get; set; }
    }
}