namespace EA.Iws.Web.Areas.Reports.ViewModels.Compliance
{
    using Core.Reports;
    using Core.Reports.Compliance;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public ReportInputParametersViewModel InputParameters { get; set; }

        public IndexViewModel()
        {
            InputParameters = new ReportInputParametersViewModel(typeof(ComplianceReportDates),
                typeof(ComplianceTextFields),
                typeof(TextFieldOperator));
        }
    }
}