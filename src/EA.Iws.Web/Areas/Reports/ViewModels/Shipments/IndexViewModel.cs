namespace EA.Iws.Web.Areas.Reports.ViewModels.Shipments
{
    using System;
    using Core.Reports;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public ReportInputParametersViewModel InputParameters { get; set; }

        public ShipmentsReportDates? DateType
        {
            get { return InputParameters.TryParse<ShipmentsReportDates>(InputParameters.SelectedDate); }
        }

        public ShipmentReportTextFields? TextFieldType
        {
            get { return InputParameters.TryParse<ShipmentReportTextFields>(InputParameters.SelectedTextField); }
        }

        public TextFieldOperator? OperatorType
        {
            get { return InputParameters.TryParse<TextFieldOperator>(InputParameters.SelectedOperator); }
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
            InputParameters = new ReportInputParametersViewModel(typeof(ShipmentsReportDates),
                typeof(ShipmentReportTextFields), typeof(TextFieldOperator));
        }
    }
}