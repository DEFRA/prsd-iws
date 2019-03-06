namespace EA.Iws.Web.Areas.Reports.ViewModels.FreedomOfInformation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Core.Reports;
    using Core.Reports.FOI;
    using EA.Iws.Web.ViewModels.Shared;

    public class ColumnSelectionViewModel
    {
        public ReportOutputParametersViewModel FOIReportColumns { get; set; }

        //public FOIReportDates FoiDateType { get; set; }
        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }
        //public FOIReportTextFields? FoiTextFieldType { get; set; }
        //public TextFieldOperator? FoiOperatorType { get; set; }
        //public string SearchText { get; set; }

        public ColumnSelectionViewModel()
        {
        }
        public ColumnSelectionViewModel(FOIReportDates dateType,
            DateTime from,
            DateTime to,
            FOIReportTextFields? textFieldType,
            TextFieldOperator? operatorType, string searchText, CheckBoxCollectionViewModel foiOutputColumns)
        {
            //FoiDateType = dateType;
            //FromDate = from;
            //ToDate = to;
            //FoiTextFieldType = textFieldType;
            //FoiOperatorType = operatorType;
            //SearchText = searchText;
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