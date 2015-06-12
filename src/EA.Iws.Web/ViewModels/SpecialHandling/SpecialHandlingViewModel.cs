namespace EA.Iws.Web.ViewModels.SpecialHandling
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;

    public class SpecialHandlingViewModel : IValidatableObject
    {
        public bool? HasSpecialHandlingRequirements { get; set; }

        public Guid NotificationId { get; set; }

        [Display(Name = "Provide details of the special handling requirements")]
        [RequiredIf("HasSpecialHandlingRequirements", true, "Please provide details of the special handling requirements")]
        public string SpecialHandlingDetails { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!HasSpecialHandlingRequirements.HasValue)
            {
                yield return new ValidationResult("Please answer this question.", new[] { "HasSpecialHandlingRequirements" });
            }
        }
    }
}