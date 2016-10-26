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
            ConsentValidFrom = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidTo = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
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
        public OptionalDateInputViewModel ConsentValidFrom { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ConsentValidTo")]
        public OptionalDateInputViewModel ConsentValidTo { get; set; }

        public SelectList Areas { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EwcCode) && string.IsNullOrWhiteSpace(ProducerName)
                && string.IsNullOrWhiteSpace(ImporterName) && string.IsNullOrWhiteSpace(ImportCountryName)
                && !LocalAreaId.HasValue && !ConsentValidTo.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.NoSearchCriteriaCompleted);
            }
        }
    }
}