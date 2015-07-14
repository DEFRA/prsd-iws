namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteOperations
{
    using System;
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class TechnologyEmployedViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public IEnumerable OperationCodes { get; set; }

        [Display(Name = "I will provide these details in an annex when I submit my notification")]
        public bool AnnexProvided { get; set; }

        [Display(Name = "Display name for details")]
        [StringLength(70, ErrorMessage = "This description cannot be longer than 70 characters.")]
        public string Details { get; set; }

        public string FurtherDetails { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Details))
            {
                yield return new ValidationResult("Please enter a description of the technologies used.", new[] { "Details" });
            }
            if (AnnexProvided && !(string.IsNullOrEmpty(FurtherDetails)))
            {
                yield return new ValidationResult("If you select that you are providing the details in a separate annex do not enter any details here.", new[] { "FurtherDetails" });
            }
        }
    }
}