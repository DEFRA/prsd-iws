namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.StateOfImport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Prsd.Core.Validation;
    using Views.StateOfImport;
    using Web.ViewModels.Shared;

    public class StateOfImportViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Country", ResourceType = typeof(StateOfImportResources))]
        public Guid? CountryId { get; set; }

        [Display(Name = "EntryPoint", ResourceType = typeof(StateOfImportResources))]
        [RequiredIf("ShowNextSection", true, ErrorMessageResourceName = "EntryPointRequired", ErrorMessageResourceType = typeof(StateOfImportResources))]
        public Guid? EntryOrExitPointId { get; set; }

        public bool ShowNextSection { get; set; }

        [Display(Name = "CA", ResourceType = typeof(StateOfImportResources))]
        [RequiredIf("ShowNextSection", true, ErrorMessageResourceName = "CARequired", ErrorMessageResourceType = typeof(StateOfImportResources))]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }
        
        public Guid? StateOfExportCountryId { get; set; }

        public IList<Guid> TransitStateCountryIds { get; set; }

        public SelectList Countries { get; set; }

        public SelectList EntryPoints { get; set; }

        public StateOfImportViewModel()
        {
            TransitStateCountryIds = new List<Guid>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CountryId.HasValue && CountryId == StateOfExportCountryId)
            {
                yield return new ValidationResult(StateOfImportResources.ImportExportCountryShouldNotSame, new[] { "CountryId" });
            }

            if (CountryId.HasValue && TransitStateCountryIds.Contains(CountryId.Value))
            {
                yield return new ValidationResult(StateOfImportResources.ImportTransitCountryShouldNotSame, new[] { "CountryId" });
            }
        }
    }
}