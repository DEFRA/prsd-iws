namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.WasteCodes;

    public class EnterWasteCodesViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public IEnumerable<WasteCodeData> Codes { get; set; }

        public string SelectedCode { get; set; }

        public List<WasteCodeData> SelectedCodesList { get; set; }

        [Display(Name = "Not applicable")]
        public bool IsNotApplicable { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(SelectedCode) && (SelectedCodesList == null || !SelectedCodesList.Any()) && !IsNotApplicable)
            {
                yield return new ValidationResult("Please enter a code or select not applicable", new[] { "SelectedCode" });
            }
        }
    }
}