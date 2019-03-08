namespace EA.Iws.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ReportOutputParametersViewModel : IValidatableObject
    {
        public CheckBoxCollectionViewModel ReportColumns { get; set; }

        public IList<string> SelectedColumns
        {
            get
            {
                return ReportColumns
                    .PossibleValues
                    .Where(x => x.Selected)
                    .Select(x => x.Value)
                    .ToList();
            }
        }

        public IList<string> ColumnsToHide
        {
            get
            {
                return ReportColumns
                    .PossibleValues
                    .Where(x => !x.Selected)
                    .Select(x => x.Value)
                    .ToList();
            }
        }

        public string DateType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TextFieldType { get; set; }
        public string OperatorType { get; set; }
        public string SearchText { get; set; }

        public ReportOutputParametersViewModel()
        {
            ReportColumns = new CheckBoxCollectionViewModel();
        }

        public ReportOutputParametersViewModel(string dateType,
            DateTime from,
            DateTime to,
            string textFieldType,
            string operatorType, string searchText, CheckBoxCollectionViewModel outputColumns)
        {
            DateType = dateType;
            FromDate = from;
            ToDate = to;
            TextFieldType = textFieldType;
            OperatorType = operatorType;
            SearchText = searchText;
            ReportColumns = outputColumns;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SelectedColumns.Any())
            {
                yield return new ValidationResult("Select the information that you wish to show in the report", new[] { "ReportColumns" });
            }
        }

        public string GetColumnsToHide()
        {
            if (ColumnsToHide.Count > 0)
            {
                return string.Join(",", ColumnsToHide);
            }
            return string.Empty;
        }
    }       
}