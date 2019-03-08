namespace EA.Iws.Web.Areas.Reports.ViewModels.ExportNotifications
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        [Display(Name = "From", ResourceType = typeof(IndexViewModelResources))]
        public RequiredDateInputViewModel From { get; set; }

        [Display(Name = "To", ResourceType = typeof(IndexViewModelResources))]
        public RequiredDateInputViewModel To { get; set; }

        public IndexViewModel()
        {
            From = new RequiredDateInputViewModel();
            To = new RequiredDateInputViewModel();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.AsDateTime() > To.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "FromDate" });
            }
        }
    }
}