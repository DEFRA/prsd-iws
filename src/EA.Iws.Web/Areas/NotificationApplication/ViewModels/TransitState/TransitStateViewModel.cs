namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.TransitState
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Prsd.Core.Validation;
    using Web.ViewModels.Shared;

    public class TransitStateViewModel : IValidatableObject
    {
        public int? OrdinalPosition { get; set; }

        [Required]
        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        public bool ShowNextSection { get; set; }

        public SelectList Countries { get; set; }

        public SelectList EntryOrExitPoints { get; set; }

        [Display(Name = "Entry point")]
        [RequiredIf("ShowNextSection", true, "The entry point is required")]
        public Guid? EntryPointId { get; set; }

        [Display(Name = "Exit point")]
        [RequiredIf("ShowNextSection", true, "The exit point is required")]
        public Guid? ExitPointId { get; set; }

        [Display(Name = "Competent authority")]
        [RequiredIf("ShowNextSection", true, "The competent authority is required")]
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
                yield return new ValidationResult("Transit country may not be the same as export country.", new[] { "CountryId" });
            }

            if (CountryId == StateOfImportCountryId)
            {
                yield return new ValidationResult("Transit country may not be the same as import country.", new[] { "CountryId" });
            }

            if (TransitStateCountryIds != null && TransitStateCountryIds.Contains(CountryId.GetValueOrDefault()))
            {
                yield return new ValidationResult("Transit country may not be the same as another transit country.", new[] { "CountryId" });
            }
        }
    }
}