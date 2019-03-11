namespace EA.Iws.Web.Areas.Reports.ViewModels.ExportStats
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        public IndexViewModel()
        {
            From = new OptionalDateInputViewModel(true);
            To = new OptionalDateInputViewModel(true);
        }

        [Display(Name = "From", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "FromRequired",
            ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel From { get; set; }

        [Display(Name = "To", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ToRequired",
            ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel To { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.AsDateTime() > To.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "FromDate" });
            }
        }
    }
}