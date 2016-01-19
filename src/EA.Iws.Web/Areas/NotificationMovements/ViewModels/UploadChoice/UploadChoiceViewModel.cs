namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.UploadChoice
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UploadChoiceViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        [Display(Name = "Number", ResourceType = typeof(UploadChoiceViewModelResources))]
        public int? Number { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Number.HasValue)
            {
                yield return new ValidationResult(UploadChoiceViewModelResources.Number, new[] { "Number" });
            }

            if (Number.GetValueOrDefault(0) <= 0)
            {
                yield return new ValidationResult(UploadChoiceViewModelResources.ValidatePositive, new[] { "Number" });
            }
        }
    }
}