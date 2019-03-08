namespace EA.Iws.Web.Areas.Reports.ViewModels.FreedomOfInformation
{
    using System;
    using Core.Reports;
    using Core.Reports.FOI;
    using EA.Iws.Web.ViewModels.Shared;

    public class ColumnSelectionViewModel
    {
        public ReportOutputParametersViewModel FOIReportColumns { get; set; }

        public ColumnSelectionViewModel()
        {
        }
        public ColumnSelectionViewModel(FOIReportDates dateType,
            DateTime from,
            DateTime to,
            FOIReportTextFields? textFieldType,
            TextFieldOperator? operatorType, string searchText, CheckBoxCollectionViewModel foiOutputColumns)
        {
            FOIReportColumns = new ReportOutputParametersViewModel()
            {
                DateType = dateType.ToString(),
                FromDate = from,
                ToDate = to,
                TextFieldType = textFieldType.ToString(),
                OperatorType = operatorType.ToString(),
                SearchText = searchText,
                ReportColumns = foiOutputColumns
            };
         }
    }
}