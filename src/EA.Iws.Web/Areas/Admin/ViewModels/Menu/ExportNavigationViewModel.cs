namespace EA.Iws.Web.Areas.Admin.ViewModels.Menu
{
    using Core.NotificationAssessment;
    using Infrastructure;

    public class ExportNavigationViewModel
    {
        public NotificationAssessmentSummaryInformationData Data { get; set; }

        public AdminLinksViewModel AdminLinksModel { get; set; }

        public ExportNavigationSection ActiveSection { get; set; }

        public bool ShowAssessmentDecision { get; set; }

        public bool ShowKeyDatesOverride { get; set; }

        public bool ShowFinancialGuaranteeDatesOverride { get; set; }
    }
}