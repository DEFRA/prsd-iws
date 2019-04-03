namespace EA.Iws.Web.Areas.Reports.ViewModels.Shipments
{
    using System;
    using Core.Reports;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public ReportInputParametersViewModel InputParameters { get; set; }

        public IndexViewModel()
        {
            InputParameters = new ReportInputParametersViewModel(typeof(ShipmentsReportDates),
                typeof(ShipmentReportTextFields), typeof(TextFieldOperator));
        }
    }
}