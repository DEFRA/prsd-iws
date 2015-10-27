namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.StateOfImport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Prsd.Core.Validation;
    using Web.ViewModels.Shared;

    public class StateOfImportViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        [Display(Name = "Entry point")]
        [RequiredIf("ShowNextSection", true, ErrorMessage = "The entry point is required")]
        public Guid? EntryOrExitPointId { get; set; }

        public bool ShowNextSection { get; set; }

        [Display(Name = "Competent authority")]
        [RequiredIf("ShowNextSection", true, ErrorMessage = "The competent authority is required")]
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
                yield return new ValidationResult("State of import country may not be the same as the state of export", new[] { "CountryId" });
            }

            if (CountryId.HasValue && TransitStateCountryIds.Contains(CountryId.Value))
            {
                yield return new ValidationResult("State of import country may not be the same as a transit state", new[] { "CountryId" });
            }
        }
    }
}