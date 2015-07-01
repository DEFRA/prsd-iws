namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.WasteCodes;

    public class UnNumberViewModel : IValidatableObject
    {
        public UnNumberViewModel()
        {
            SelectedUnNumbers = new List<WasteCodeData>();
            SelectedCustomCodes = new List<string>();
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<WasteCodeData> UnNumbers { get; set; }

        public List<WasteCodeData> SelectedUnNumbers { get; set; }

        [Display(Name = "UN Number")]
        public string SelectedUnNumber { get; set; }

        [Display(Name = "Custom codes")]
        public string SelectedCustomCode { get; set; }
        
        public List<string> SelectedCustomCodes { get; set; }

        public string TypeOfCodeAdded { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(SelectedUnNumber) && (SelectedUnNumbers == null || !SelectedUnNumbers.Any()))
            {
                yield return new ValidationResult("Please enter a UN number or select not applicable", new[] { "SelectedUnNumber" });
            }
        }
    }
}