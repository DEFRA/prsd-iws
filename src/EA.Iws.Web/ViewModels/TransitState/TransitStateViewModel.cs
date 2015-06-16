namespace EA.Iws.Web.ViewModels.TransitState
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Prsd.Core.Validation;
    using Shared;

    public class TransitStateViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        public bool IsCountrySelected { get; set; }

        public SelectList Countries { get; set; }

        [Display(Name = "Entry point")]
        [RequiredIf("IsCountrySelected", true, "The entry point is required")]
        public Guid? EntryPointId { get; set; }

        [Display(Name = "Exit point")]
        [RequiredIf("IsCountrySelected", true, "The exit point is required")]
        public Guid? ExitPointId { get; set; }

        [Display(Name = "Competent authority")]
        [RequiredIf("IsCountrySelected", true, "The competent authority is required")]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }

        public Guid? StateOfImportCountryId { get; set; }

        public IList<Guid> TransitStateCountryIds { get; set; }

        public Guid? StateOfExportCountryId { get; set; }

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

            if (ExitPointId == EntryPointId && IsCountrySelected)
            {
                yield return new ValidationResult("Exit and entry point must not be the same.", new[] { "ExitPointId" });
            }
        }
    }
}