namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteOperations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Views.WasteOperations;

    public class TechnologyEmployedViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public IEnumerable OperationCodes { get; set; }

        [Display(Name = "AnnexProvided", ResourceType = typeof(TechnologyEmployedResources))]
        public bool AnnexProvided { get; set; }

        [Display(Name = "TechnologyDescription", ResourceType = typeof(TechnologyEmployedResources))]
        [StringLength(70, ErrorMessageResourceName = "DetailsMaxLengthErrorMessage", ErrorMessageResourceType = typeof(TechnologyEmployedResources))]
        public string Details { get; set; }

        [Display(Name = "FurtherDetails", ResourceType = typeof(TechnologyEmployedResources))]
        public string FurtherDetails { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Details))
            {
                yield return new ValidationResult(TechnologyEmployedResources.DetailsRequired, new[] { "Details" });
            }

            if (AnnexProvided && !(string.IsNullOrWhiteSpace(FurtherDetails)))
            {
                yield return new ValidationResult(TechnologyEmployedResources.FurtherDetailsRequired, new[] { "FurtherDetails" });
            }
        }
    }
}