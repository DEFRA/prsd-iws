namespace EA.Iws.Web.Areas.Reports.ViewModels.Producer
{
    using System;
    using Core.Reports;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public ReportInputParametersViewModel InputParameters { get; set; }

        public IndexViewModel()
        {
            InputParameters = new ReportInputParametersViewModel(typeof(ProducerReportDates),
                typeof(ProducerReportTextFields), 
                typeof(TextFieldOperator));
        }
    }
}