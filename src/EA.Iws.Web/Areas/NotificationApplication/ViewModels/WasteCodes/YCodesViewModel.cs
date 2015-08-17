namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.WasteCodes;

    public class YCodesViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public IEnumerable<WasteCodeData> Ycodes { get; set; }

        public string SelectedYcode { get; set; }

        public List<WasteCodeData> SelectedYcodesList { get; set; }

        [Display(Name = "Not applicable")]
        public bool IsNotApplicable { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(SelectedYcode) && (SelectedYcodesList == null || !SelectedYcodesList.Any()) && !IsNotApplicable)
            {
                yield return new ValidationResult("Please enter a Y code or select not applicable", new[] { "SelectedYcode" });
            }
        }
    }
}