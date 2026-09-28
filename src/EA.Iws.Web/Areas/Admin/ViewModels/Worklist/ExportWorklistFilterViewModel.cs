namespace EA.Iws.Web.Areas.Admin.ViewModels.Worklist
{
    using Core.NotificationAssessment;

    public class ExportWorklistFilterViewModel
    {
        public string NotificationNumber { get; set; }

        public string Officer { get; set; }

        public NotificationStatus[] SelectedStatuses { get; set; }

        // Indicates the filter form has been submitted at least once (as opposed to a fresh page load).
        public bool HasSubmitted { get; set; }
    }
}