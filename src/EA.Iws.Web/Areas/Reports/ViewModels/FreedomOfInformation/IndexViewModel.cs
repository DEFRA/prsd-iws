namespace EA.Iws.Web.Areas.Reports.ViewModels.FreedomOfInformation
{
    using System;
    using Core.Reports;
    using Core.Reports.FOI;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public ReportInputParametersViewModel InputParameters { get; set; }

        public IndexViewModel()
        {
            InputParameters = new ReportInputParametersViewModel(typeof(FOIReportDates),
                typeof(FOIReportTextFields),
                typeof(TextFieldOperator));
        }
    }
}