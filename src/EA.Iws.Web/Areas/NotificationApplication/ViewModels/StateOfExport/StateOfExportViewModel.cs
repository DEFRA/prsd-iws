namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.StateOfExport
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;
    using Web.ViewModels.Shared;

    public class StateOfExportViewModel
    {
        [Display(Name = "Country")]
        public Guid CountryId { get; set; }

        [Display(Name = "Exit point")]
        [RequiredIf("LoadNextSection", true, "The exit point is required")]
        public Guid? EntryOrExitPointId { get; set; }

        public bool LoadNextSection { get; set; }

        [Display(Name = "Competent authority")]
        [RequiredIf("LoadNextSection", true, "The competent authority is required")]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }
    }
}