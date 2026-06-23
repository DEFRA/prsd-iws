namespace EA.Iws.Web.Areas.Admin.ViewModels.Worklist
{
    using Core.NotificationAssessment;

    public class WorklistViewModel
    {
        public ExportWorklistFilterViewModel ExportsFilter { get; set; }

        public ExportWorklistResult ExportsResult { get; set; }

        public WorklistViewModel()
        {
            ExportsFilter = new ExportWorklistFilterViewModel();
        }
    }
}