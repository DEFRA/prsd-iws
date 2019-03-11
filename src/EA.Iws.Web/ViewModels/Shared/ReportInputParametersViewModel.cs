namespace EA.Iws.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Prsd.Core.Helpers;

    public class ReportInputParametersViewModel : IValidatableObject
    {
        [Display(Name = "DateType", ResourceType = typeof(ReportInputParametersResources))]
        [Required(ErrorMessageResourceName = "DateTypeRequired", ErrorMessageResourceType = typeof(ReportInputParametersResources))]
        public string SelectedDate { get; set; }

        [Display(Name = "TextFieldType", ResourceType = typeof(ReportInputParametersResources))]
        public string SelectedTextField { get; set; }

        [Display(Name = "TextFieldOperator", ResourceType = typeof(ReportInputParametersResources))]
        public string SelectedOperator { get; set; }

        [Display(Name = "FromDate", ResourceType = typeof(ReportInputParametersResources))]
        public RequiredDateInputViewModel FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(ReportInputParametersResources))]
        public RequiredDateInputViewModel ToDate { get; set; }

        [Display(Name = "TextSearch", ResourceType = typeof(ReportInputParametersResources))]
        public string TextSearch { get; set; }

        public SelectList DateSelectList { get; set; }

        public SelectList TextFieldSelectList { get; set; }

        public SelectList TextOperatorSelectList { get; set; }

        public ReportInputParametersViewModel(Type dateType, Type textFieldType, Type textOperatorType)
        {
            FromDate = new RequiredDateInputViewModel();
            ToDate = new RequiredDateInputViewModel();

            var textFieldTypes = EnumHelper.GetValues(textFieldType);
            textFieldTypes.Add(-1, "View all");
            var orderedTextFieldTypes = textFieldTypes.OrderBy(x => x.Key);

            DateSelectList = new SelectList(EnumHelper.GetValues(dateType), "Key", "Value", null);
            TextFieldSelectList = new SelectList(orderedTextFieldTypes, "Key", "Value", null);
            TextOperatorSelectList = new SelectList(EnumHelper.GetValues(textOperatorType), "Key", "Value", null);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromDate.AsDateTime() > ToDate.AsDateTime())
            {
                yield return new ValidationResult(ReportInputParametersResources.FromDateBeforeToDate, new[] { "FromDate" });
            }

            if (string.IsNullOrEmpty(SelectedTextField) || SelectedTextField != "-1")
            {
                if (string.IsNullOrEmpty(SelectedTextField))
                {
                    yield return
                        new ValidationResult(ReportInputParametersResources.TextFieldRequired, new[] { "SelectedTextField" });
                }

                if (!string.IsNullOrEmpty(SelectedTextField) && string.IsNullOrEmpty(SelectedOperator))
                {
                    yield return
                        new ValidationResult(ReportInputParametersResources.TextFieldOperatorRequired, new[] { "SelectedOperator" });
                }

                if (!string.IsNullOrEmpty(SelectedTextField) && string.IsNullOrEmpty(TextSearch))
                {
                    yield return
                        new ValidationResult(ReportInputParametersResources.TextSearchRequired, new[] { "TextSearch" });
                }
            }
        }     
    }
}