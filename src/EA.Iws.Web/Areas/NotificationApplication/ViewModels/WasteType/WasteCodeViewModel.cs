namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Requests.WasteType;

    public class WasteCodeViewModel : IValidatableObject
    {
        public IEnumerable<WasteCodeData> WasteCodes { get; set; }

        [Required]
        [Display(Name = "Basel annex VIII/IX or OECD code")]
        public string SelectedWasteCode { get; set; }

        public Guid NotificationId { get; set; }

        public IEnumerable<WasteCodeData> EwcCodes { get; set; }

        [Display(Name = "EWC Code")]
        public List<WasteCodeData> SelectedEwcCodes { get; set; }

        public string SelectedEwcCode { get; set; }

        public string Command { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Command.Equals("save") && (SelectedEwcCodes == null || !SelectedEwcCodes.Any()))
            {
                yield return new ValidationResult("Please enter a EWC code");
            }
        }
    }
}