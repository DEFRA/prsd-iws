namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.CustomWasteCode
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;
    using Views.CustomWasteCode;

    public class CustomWasteCodesViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        [RequiredIf("ImportNationalCodeNotApplicable", false, ErrorMessageResourceName = "ImportNationalCodeRequired", ErrorMessageResourceType = typeof(CustomWasteCodeResources))]
        [Display(Name = "ImportNationalCode", ResourceType = typeof(CustomWasteCodeResources))]
        public string ImportNationalCode { get; set; }

        [Display(Name = "NotApplicable", ResourceType = typeof(CustomWasteCodeResources))]
        public bool ImportNationalCodeNotApplicable { get; set; }

        [RequiredIf("ExportNationalCodeNotApplicable", false, ErrorMessageResourceName = "ExportNationalCodeRequired", ErrorMessageResourceType = typeof(CustomWasteCodeResources))]
        [Display(Name = "ExportNationalCode", ResourceType = typeof(CustomWasteCodeResources))]
        public string ExportNationalCode { get; set; }

        [Display(Name = "NotApplicable", ResourceType = typeof(CustomWasteCodeResources))]
        public bool ExportNationalCodeNotApplicable { get; set; }

        [RequiredIf("CustomsCodeNotApplicable", false, ErrorMessageResourceName = "CustomsCodeRequired", ErrorMessageResourceType = typeof(CustomWasteCodeResources))]
        [Display(Name = "CustomsCode", ResourceType = typeof(CustomWasteCodeResources))]
        public string CustomsCode { get; set; }

        [Display(Name = "NotApplicable", ResourceType = typeof(CustomWasteCodeResources))]
        public bool CustomsCodeNotApplicable { get; set; }

        [Display(Name = "OtherCode", ResourceType = typeof(CustomWasteCodeResources))]
        [RequiredIf("OtherCodeNotApplicable", false, ErrorMessageResourceName = "OtherCodeRequired", ErrorMessageResourceType = typeof(CustomWasteCodeResources))]
        public string OtherCode { get; set; }

        [Display(Name = "NotApplicable", ResourceType = typeof(CustomWasteCodeResources))]
        public bool OtherCodeNotApplicable { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(ImportNationalCode) && ImportNationalCodeNotApplicable)
            {
                yield return new ValidationResult(CustomWasteCodeResources.DoNotEnterCodeIfNASelected, new[] { "ImportNationalCode" });
            }

            if (!string.IsNullOrWhiteSpace(ExportNationalCode) && ExportNationalCodeNotApplicable)
            {
                yield return new ValidationResult(CustomWasteCodeResources.DoNotEnterCodeIfNASelected, new[] { "ExportNationalCode" });
            }

            if (!string.IsNullOrWhiteSpace(CustomsCode) && CustomsCodeNotApplicable)
            {
                yield return new ValidationResult(CustomWasteCodeResources.DoNotEnterCodeIfNASelected, new[] { "CustomsCode" });
            }

            if (!string.IsNullOrWhiteSpace(OtherCode) && OtherCodeNotApplicable)
            {
                yield return new ValidationResult(CustomWasteCodeResources.DoNotEnterCodeIfNASelected, new[] { "OtherCode" });
            }
        }
    }
}