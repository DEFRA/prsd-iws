namespace EA.Iws.Web.ViewModels.WasteOperations
{
    using System;
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class TechnologyEmployedViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public IEnumerable OperationCodes { get; set; }

        [Display(Name = "I will provide these details in an annex when I submit my notification")]
        public bool AnnexPorvided { get; set; }

        [Display(Name = "Display name for details")]
        public string Details { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!AnnexPorvided && string.IsNullOrEmpty(Details))
            {
                yield return new ValidationResult("Please enter details or indicate that you will be providing them in a separate annex.");
            }

            if (AnnexPorvided && !(string.IsNullOrEmpty(Details)))
            {
                yield return new ValidationResult("If you select that you are providing the details in a separate annex do not enter any details here.");
            }
        }
    }
}