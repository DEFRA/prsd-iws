namespace EA.Iws.Web.Areas.Reports.ViewModels.Shipments
{
    using System;
    using Core.Reports;
    using EA.Iws.Web.ViewModels.Shared;

    public class ColumnSelectionViewModel
    {
        public ReportOutputParametersViewModel ShipmentReportColumns { get; set; }

        public ColumnSelectionViewModel()
        {
        }
        public ColumnSelectionViewModel(ShipmentsReportDates dateType,
            DateTime from,
            DateTime to,
            ShipmentReportTextFields? textFieldType,
            TextFieldOperator? operatorType, string searchText, CheckBoxCollectionViewModel shipmentOutputColumns)
        {
            ShipmentReportColumns = new ReportOutputParametersViewModel()
            {
                DateType = dateType.ToString(),
                FromDate = from,
                ToDate = to,
                TextFieldType = textFieldType.ToString(),
                OperatorType = operatorType.ToString(),
                SearchText = searchText,
                ReportColumns = shipmentOutputColumns
            };
        }
    }
}