namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.WasteCodes;

    public class BaselEwcCodeViewModel : IValidatableObject
    {
        public BaselEwcCodeViewModel()
        {
            SelectedEwcCodes = new List<WasteCodeData>();
        }

        public IEnumerable<WasteCodeData> BaselOecdCodes { get; set; }

        [Display(Name = "Basel annex VIII/IX or OECD code")]
        public string SelectedBaselCode { get; set; }

        public Guid NotificationId { get; set; }

        public IEnumerable<WasteCodeData> EwcCodes { get; set; }

        public List<WasteCodeData> SelectedEwcCodes { get; set; }

        [Display(Name = "European waste catalogue (EWC) Codes")]
        public string SelectedEwcCode { get; set; }

        public string TypeOfCodeAdded { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(SelectedBaselCode) && (BaselOecdCodes == null || !BaselOecdCodes.Any()))
            {
                yield return new ValidationResult("Please enter a Basel code", new[] { "SelectedBaselCode" });
            }

            if (string.IsNullOrEmpty(SelectedEwcCode) && (SelectedEwcCodes == null || !SelectedEwcCodes.Any()))
            {
                yield return new ValidationResult("Please enter an EWC code", new[] { "SelectedEwcCode" });
            }
        }
    }
}