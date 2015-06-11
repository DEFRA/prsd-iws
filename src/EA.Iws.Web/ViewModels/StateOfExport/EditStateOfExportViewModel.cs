namespace EA.Iws.Web.ViewModels.StateOfExport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Shared;

    public class EditStateOfExportViewModel
    {
        public Guid NotificationId { get; set; }

        [Required]
        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        public Guid? CompetentAuthorityId { get; set; }

        [Required]
        [Display(Name = "Exit point")]
        public Guid? ExitPointId { get; set; }

        public SelectList Countries { get; set; }

        public SelectList ExitPoints { get; set; }
        
        [Required]
        [Display(Name = "Competent authority")]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }

        public bool ShowNextSection { get; set; }

        public Guid? StateOfImportCountryId { get; set; }

        public IList<Guid> TransitStateCompetentAuthorityIds { get; set; }
    }
}