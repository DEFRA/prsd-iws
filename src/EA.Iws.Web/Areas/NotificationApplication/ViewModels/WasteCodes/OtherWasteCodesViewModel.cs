namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;

    public class OtherWasteCodesViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        [RequiredIf("ImportNationalCodeNotApplicable", false, "Please enter a national code in country of import or select not applicable")]
        [Display(Name = "National code in country of import")]
        public string ImportNationalCode { get; set; }

        [Display(Name = "Not applicable")]
        public bool ImportNationalCodeNotApplicable { get; set; }

        [RequiredIf("ExportNationalCodeNotApplicable", false, "Please enter a national code in country of export or select not applicable")]
        [Display(Name = "National code in country of export")]
        public string ExportNationalCode { get; set; }

        [Display(Name = "Not applicable")]
        public bool ExportNationalCodeNotApplicable { get; set; }

        [RequiredIf("CustomsCodeNotApplicable", false, "Please enter a customs code or select not applicable")]
        [Display(Name = "Customs code")]
        public string CustomsCode { get; set; }

        [Display(Name = "Not applicable")]
        public bool CustomsCodeNotApplicable { get; set; }

        [Display(Name = "Other code(s)")]
        [RequiredIf("OtherCodeNotApplicable", false, "Please enter other code(s) or select not applicable")]
        public string OtherCode { get; set; }

        [Display(Name = "Not applicable")]
        public bool OtherCodeNotApplicable { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(ImportNationalCode) && ImportNationalCodeNotApplicable)
            {
                yield return new ValidationResult("If not applicable is selected please do not enter a code", new[] { "ImportNationalCode" });
            }

            if (!string.IsNullOrWhiteSpace(ExportNationalCode) && ExportNationalCodeNotApplicable)
            {
                yield return new ValidationResult("If not applicable is selected please do not enter a code", new[] { "ExportNationalCode" });
            }

            if (!string.IsNullOrWhiteSpace(CustomsCode) && CustomsCodeNotApplicable)
            {
                yield return new ValidationResult("If not applicable is selected please do not enter a code", new[] { "CustomsCode" });
            }

            if (!string.IsNullOrWhiteSpace(OtherCode) && OtherCodeNotApplicable)
            {
                yield return new ValidationResult("If not applicable is selected please do not enter a code", new[] { "OtherCode" });
            }
        }
    }
}