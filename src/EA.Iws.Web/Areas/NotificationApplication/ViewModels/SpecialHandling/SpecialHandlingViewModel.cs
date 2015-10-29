namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.SpecialHandling
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;
    using Views.SpecialHandling;

    public class SpecialHandlingViewModel : IValidatableObject
    {
        public bool? HasSpecialHandlingRequirements { get; set; }

        public Guid NotificationId { get; set; }

        [Display(Name = "SpecialHandlingDetails", ResourceType = typeof(SpecialHandlingResources))]
        [RequiredIf("HasSpecialHandlingRequirements", true, ErrorMessageResourceName = "SpecialHandlingDetailsRequired", ErrorMessageResourceType = typeof(SpecialHandlingResources))]
        public string SpecialHandlingDetails { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!HasSpecialHandlingRequirements.HasValue)
            {
                yield return new ValidationResult(SpecialHandlingResources.HasSpecialHandlingRequirements, new[] { "HasSpecialHandlingRequirements" });
            }
        }
    }
}