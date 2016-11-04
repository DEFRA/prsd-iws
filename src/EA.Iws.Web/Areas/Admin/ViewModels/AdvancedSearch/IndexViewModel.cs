namespace EA.Iws.Web.Areas.Admin.ViewModels.AdvancedSearch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        public IndexViewModel()
        {
            ConsentValidFromStart = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidFromEnd = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidToStart = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidToEnd = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
        }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "EwcCode")]
        public string EwcCode { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ProducerName")]
        public string ProducerName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ImporterName")]
        public string ImporterName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ImportCountryName")]
        public string ImportCountryName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "LocalArea")]
        public Guid? LocalAreaId { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ConsentValidFrom")]
        public OptionalDateInputViewModel ConsentValidFromStart { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "To")]
        public OptionalDateInputViewModel ConsentValidFromEnd { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ConsentValidTo")]
        public OptionalDateInputViewModel ConsentValidToStart { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "To")]
        public OptionalDateInputViewModel ConsentValidToEnd { get; set; }

        public SelectList Areas { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EwcCode) && string.IsNullOrWhiteSpace(ProducerName)
                && string.IsNullOrWhiteSpace(ImporterName) && string.IsNullOrWhiteSpace(ImportCountryName)
                && !LocalAreaId.HasValue && !(ConsentValidToStart.IsCompleted && ConsentValidToEnd.IsCompleted))
            {
                yield return new ValidationResult(IndexViewModelResources.NoSearchCriteriaCompleted);
            }

            if (ConsentValidToStart.IsCompleted && !ConsentValidToEnd.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterEndDate, new[] { "ConsentValidToEnd" });
            }

            if (ConsentValidToEnd.IsCompleted && !ConsentValidToStart.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterStartDate, new[] { "ConsentValidToStart" });
            }
        }
    }
}