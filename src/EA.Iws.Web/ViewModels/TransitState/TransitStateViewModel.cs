namespace EA.Iws.Web.ViewModels.TransitState
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;
    using Shared;

    public class TransitStateViewModel
    {
        [Required]
        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        public bool LoadNextSection { get; set; }

        [Display(Name = "Entry point")]
        [RequiredIf("LoadNextSection", true, "The entry point is required")]
        public Guid? EntryPointId { get; set; }

        [Display(Name = "Exit point")]
        [RequiredIf("LoadNextSection", true, "The exit point is required")]
        public Guid? ExitPointId { get; set; }

        [Display(Name = "Competent authority")]
        [RequiredIf("LoadNextSection", true, "The competent authority is required")]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }
    }
}