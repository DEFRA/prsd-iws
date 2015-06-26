namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.StateOfExport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Prsd.Core.Validation;
    using Web.ViewModels.Shared;

    public class StateOfExportViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        [Display(Name = "Entry point")]
        [RequiredIf("ShowNextSection", true, "The entry point is required")]
        public Guid? EntryOrExitPointId { get; set; }

        public bool ShowNextSection { get; set; }

        [Display(Name = "Competent authority")]
        [RequiredIf("ShowNextSection", true, "The competent authority is required")]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }

        public Guid? StateOfImportCountryId { get; set; }

        public IList<Guid> TransitStateCountryIds { get; set; }

        public SelectList Countries { get; set; }

        public SelectList ExitPoints { get; set; }

        public StateOfExportViewModel()
        {
            TransitStateCountryIds = new List<Guid>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CountryId.HasValue && CountryId == StateOfImportCountryId)
            {
                yield return new ValidationResult("State of export country may not be the same as the state of import", new[] { "CountryId" });
            }

            if (CountryId.HasValue && TransitStateCountryIds.Contains(CountryId.Value))
            {
                yield return new ValidationResult("State of export country may not be the same as a transit state", new[] { "CountryId" });
            }
        }
    }
}