namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.TransitState
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Prsd.Core.Validation;
    using Views.TransitState;
    using Web.ViewModels.Shared;

    public class TransitStateViewModel : IValidatableObject
    {
        public int? OrdinalPosition { get; set; }

        [Required(ErrorMessageResourceName = "TransitCountryRequired", ErrorMessageResourceType = typeof(TransitStateResources))]
        [Display(Name = "Country", ResourceType = typeof(TransitStateResources))]
        public Guid? CountryId { get; set; }

        public bool ShowNextSection { get; set; }

        public SelectList Countries { get; set; }

        public SelectList EntryOrExitPoints { get; set; }

        [Display(Name = "EntryPoint", ResourceType = typeof(TransitStateResources))]
        [RequiredIf("ShowNextSection", true, ErrorMessageResourceName = "EntryPointRequired", ErrorMessageResourceType = typeof(TransitStateResources))]
        public Guid? EntryPointId { get; set; }

        [Display(Name = "ExitPoint", ResourceType = typeof(TransitStateResources))]
        [RequiredIf("ShowNextSection", true, ErrorMessageResourceName = "ExitPointRequired", ErrorMessageResourceType = typeof(TransitStateResources))]
        public Guid? ExitPointId { get; set; }

        [Display(Name = "CA", ResourceType = typeof(TransitStateResources))]
        [RequiredIf("ShowNextSection", true, ErrorMessageResourceName = "CARequired", ErrorMessageResourceType = typeof(TransitStateResources))]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }

        public Guid? StateOfImportCountryId { get; set; }

        public IList<Guid> TransitStateCountryIds { get; set; }

        public Guid? StateOfExportCountryId { get; set; }

        public TransitStateViewModel()
        {
            TransitStateCountryIds = new List<Guid>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CountryId == StateOfExportCountryId)
            {
                yield return new ValidationResult(TransitStateResources.TransitNotSameAsExport, new[] { "CountryId" });
            }

            if (CountryId == StateOfImportCountryId)
            {
                yield return new ValidationResult(TransitStateResources.TransitNotSameAsImport, new[] { "CountryId" });
            }

            if (TransitStateCountryIds != null && TransitStateCountryIds.Contains(CountryId.GetValueOrDefault()))
            {
                yield return new ValidationResult(TransitStateResources.TransitNotSameAsAnotherTransit, new[] { "CountryId" });
            }
        }
    }
}