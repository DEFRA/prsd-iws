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
            NotificationReceivedStart = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            NotificationReceivedEnd = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
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

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ExporterName")]
        public string ExporterName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "BaselOecdCode")]
        public string BaselOecdCode { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "FacilityName")]
        public string FacilityName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ExitPointName")]
        public string ExitPointName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "EntryPointName")]
        public string EntryPointName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "NotificationReceived")]
        public OptionalDateInputViewModel NotificationReceivedStart { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "To")]
        public OptionalDateInputViewModel NotificationReceivedEnd { get; set; }

        public SelectList Areas { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EwcCode) && string.IsNullOrWhiteSpace(ProducerName)
                && string.IsNullOrWhiteSpace(ImporterName) && string.IsNullOrWhiteSpace(ImportCountryName)
                && !LocalAreaId.HasValue && !(ConsentValidToStart.IsCompleted && ConsentValidToEnd.IsCompleted)
                && string.IsNullOrWhiteSpace(ExporterName) && string.IsNullOrWhiteSpace(BaselOecdCode)
                && string.IsNullOrWhiteSpace(FacilityName) && string.IsNullOrWhiteSpace(ExitPointName)
                && string.IsNullOrWhiteSpace(EntryPointName) && !(NotificationReceivedStart.IsCompleted && NotificationReceivedEnd.IsCompleted))
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

            if (NotificationReceivedStart.IsCompleted && !NotificationReceivedEnd.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterEndDate, new[] { "NotificationReceivedEnd" });
            }

            if (NotificationReceivedEnd.IsCompleted && !NotificationReceivedStart.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterStartDate, new[] { "NotificationReceivedStart" });
            }
        }
    }
}