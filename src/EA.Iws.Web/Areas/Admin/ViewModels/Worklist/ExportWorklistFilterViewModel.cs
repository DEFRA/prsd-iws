namespace EA.Iws.Web.Areas.Admin.ViewModels.Worklist
{
    using Core.NotificationAssessment;

    public class ExportWorklistFilterViewModel
    {
        public string NotificationNumber { get; set; }

        public string Officer { get; set; }

        public NotificationStatus[] SelectedStatuses { get; set; }
    }
}