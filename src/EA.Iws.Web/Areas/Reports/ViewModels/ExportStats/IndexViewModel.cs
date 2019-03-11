namespace EA.Iws.Web.Areas.Reports.ViewModels.ExportStats
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        public RequiredDateInputViewModel From { get; set; }

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