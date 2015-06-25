namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.WasteType;
    using Requests.WasteType;

    public class UnNumberViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public IEnumerable<WasteCodeData> UnCodes { get; set; }

        [Display(Name = "UN Number")]
        public List<WasteCodeData> SelectedUnCodes { get; set; }

        [Display(Name = "UN Number")]
        public CodeType UnCode { get; set; }

        [Display(Name = "Custom codes")]
        public List<string> CustomCodes { get; set; }

        public string SelectedCustomCode { get; set; }

        public string SelectedUnCode { get; set; }

        public string Command { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Command.Equals("save") && (SelectedUnCodes == null || !SelectedUnCodes.Any()))
            {
                yield return new ValidationResult("Please enter a UN code");
            }
        }
    }
}