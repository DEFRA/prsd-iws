namespace EA.Iws.Web.Areas.Admin.ViewModels.Worklist
{
    using Core.ImportNotificationAssessment;
    using Core.NotificationAssessment;

    public class WorklistViewModel
    {
        public ExportWorklistFilterViewModel ExportFilter { get; set; }
        public ExportWorklistResult ExportResult { get; set; }

        public ImportWorklistFilterViewModel ImportFilter { get; set; }
        public ImportWorklistResult ImportResult { get; set; }

        public NotificationStatus[] ExportStatuses { get; set; }
        public ImportNotificationStatus[] ImportStatuses { get; set; }

        public WorklistViewModel()
        {
            ExportFilter = new ExportWorklistFilterViewModel();
            ImportFilter = new ImportWorklistFilterViewModel();
        }
    }
}