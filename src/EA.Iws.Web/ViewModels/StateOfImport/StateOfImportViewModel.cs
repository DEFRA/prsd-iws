namespace EA.Iws.Web.ViewModels.StateOfImport
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;
    using Shared;

    public class StateOfImportViewModel
    {
        [Display(Name = "Country")]
        public Guid CountryId { get; set; }

        [Display(Name = "Entry point")]
        [RequiredIf("LoadNextSection", true, "The entry point is required")]
        public Guid? EntryOrExitPointId { get; set; }

        public bool LoadNextSection { get; set; }

        [Display(Name = "Competent authority")]
        [RequiredIf("LoadNextSection", true, "The competent authority is required")]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }
    }
}