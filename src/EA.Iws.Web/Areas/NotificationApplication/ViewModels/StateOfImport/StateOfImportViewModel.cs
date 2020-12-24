namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.StateOfImport
{
    using Core.Notification;
    using Prsd.Core.Validation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Views.StateOfImport;
    using Web.ViewModels.Shared;

    public class StateOfImportViewModel
    {
        [Required(ErrorMessageResourceName = "CountryOfImportRequired", ErrorMessageResourceType = typeof(StateOfImportResources))]
        [Display(Name = "Country", ResourceType = typeof(StateOfImportResources))]
        public Guid? CountryId { get; set; }

        [Display(Name = "EntryPoint", ResourceType = typeof(StateOfImportResources))]
        [RequiredIf("ShowNextSection", true, ErrorMessageResourceName = "EntryPointRequired", ErrorMessageResourceType = typeof(StateOfImportResources))]
        public Guid? EntryOrExitPointId { get; set; }

        public bool ShowNextSection { get; set; }

        [Display(Name = "CA", ResourceType = typeof(StateOfImportResources))]
        [RequiredIf("ShowNextSection", true, ErrorMessageResourceName = "CARequired", ErrorMessageResourceType = typeof(StateOfImportResources))]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }

        public Guid? StateOfExportCompetentAuthorityId { get; set; }

        public IList<Guid> TransitStateCountryIds { get; set; }

        public SelectList Countries { get; set; }

        public SelectList EntryPoints { get; set; }

        public UKCompetentAuthority NotificationCompetentAuthority { get; set; }

        public StateOfImportViewModel()
        {
            TransitStateCountryIds = new List<Guid>();
        }
    }
}