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
        public Type DateType { get; }

        [Display(Name = "TextFieldType", ResourceType = typeof(ReportInputParametersResources))]
        [Required(ErrorMessageResourceName = "TextFieldRequired", ErrorMessageResourceType = typeof(ReportInputParametersResources))]
        public Type TextFieldType { get; }

        [Display(Name = "TextFieldOperator", ResourceType = typeof(ReportInputParametersResources))]
        [Required(ErrorMessageResourceName = "TextFieldOperatorRequired", ErrorMessageResourceType = typeof(ReportInputParametersResources))]
        public Type TextOperatorType { get; }

        [Display(Name = "FromDate", ResourceType = typeof(ReportInputParametersResources))]
        public RequiredDateInputViewModel FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(ReportInputParametersResources))]
        public RequiredDateInputViewModel ToDate { get; set; }

        [Display(Name = "TextSearch", ResourceType = typeof(ReportInputParametersResources))]
        [Required(ErrorMessageResourceName = "TextSearchRequired", ErrorMessageResourceType = typeof(ReportInputParametersResources))]
        public string TextSearch { get; set; }

        public SelectList DateSelectList { get; set; }

        public SelectList TextFieldSelectList { get; set; }

        public SelectList TextOperatorSelectList { get; set; }

        public ReportInputParametersViewModel(Type date, Type textField, Type textOperator)
        {
            DateType = date;
            TextFieldType = textField;
            TextOperatorType = textOperator;

            FromDate = new RequiredDateInputViewModel(true);
            ToDate = new RequiredDateInputViewModel(true);

            var textFieldTypes = EnumHelper.GetValues(TextFieldType);
            textFieldTypes.Add(-1, "View all");
            var orderedTextFieldTypes = textFieldTypes.OrderBy(x => x.Key);

            DateSelectList = new SelectList(EnumHelper.GetValues(DateType), "Key", "Value", null);
            TextFieldSelectList = new SelectList(orderedTextFieldTypes, "Key", "Value", null);
            TextOperatorSelectList = new SelectList(EnumHelper.GetValues(TextOperatorType), "Key", "Value", null);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromDate.AsDateTime() > ToDate.AsDateTime())
            {
                yield return new ValidationResult(ReportInputParametersResources.FromDateBeforeToDate, new[] { "FromDate" });
            }
        }
    }
}