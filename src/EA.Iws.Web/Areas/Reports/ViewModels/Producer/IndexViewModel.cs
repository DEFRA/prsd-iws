namespace EA.Iws.Web.Areas.Reports.ViewModels.Producer
{
    using System;
    using Core.Reports;
    using Core.Reports.Producer;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public ReportInputParametersViewModel InputParameters { get; set; }

        public ProducerReportDates? DateType
        {
            get { return InputParameters.TryParse<ProducerReportDates>(InputParameters.SelectedDate); }
        }

        public ProducerReportTextFields? TextFieldType
        {
            get { return InputParameters.TryParse<ProducerReportTextFields>(InputParameters.SelectedDate); }
        }

        public TextFieldOperator? OperatorType
        {
            get { return InputParameters.TryParse<TextFieldOperator>(InputParameters.SelectedDate); }
        }

        public DateTime From
        {
            get { return InputParameters.FromDate.AsDateTime().Value; }
        }

        public DateTime To
        {
            get { return InputParameters.ToDate.AsDateTime().Value; }
        }

        public string TextSearch
        {
            get { return InputParameters.TextSearch; }
        }

        public IndexViewModel()
        {
            InputParameters = new ReportInputParametersViewModel(typeof(ProducerReportDates),
                typeof(ProducerReportTextFields), 
                typeof(TextFieldOperator));
        }
    }
}