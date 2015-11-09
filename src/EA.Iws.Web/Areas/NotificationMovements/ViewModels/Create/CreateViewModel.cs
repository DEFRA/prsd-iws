namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "NumberToCreate", ResourceType = typeof(CreateViewModelResources))]
        public int? NumberToCreate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NumberToCreate > 50)
            {
                yield return new ValidationResult("Please enter a value less than or equal to 50", new[] { "NumberToCreate" });
            }
        }
    }
}