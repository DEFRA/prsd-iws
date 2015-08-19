namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.StateOfExport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class StateOfExportViewModel 
    {
        public Guid? CountryId { get; set; }

        [Display(Name = "Exit point")]
        [Required(ErrorMessage = "The exit point is required")]
        public Guid? EntryOrExitPointId { get; set; }

        public string CompetentAuthorityName { get; set; }

        public Guid? StateOfImportCountryId { get; set; }

        public IList<Guid> TransitStateCountryIds { get; set; }

        public SelectList ExitPoints { get; set; }

        public StateOfExportViewModel()
        {
            TransitStateCountryIds = new List<Guid>();
        }
    }
}