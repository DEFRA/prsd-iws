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

    public class StateOfImportViewModel : IValidatableObject
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

        public Guid? StateOfExportCountryId { get; set; }

        public IList<Guid> TransitStateCountryIds { get; set; }

        public SelectList Countries { get; set; }

        public SelectList EntryPoints { get; set; }

        public UKCompetentAuthority NotificationCompetentAuthority { get; set; }

        public IList<Guid> IntraCountryExportAllowed { get; set; }

        public StateOfImportViewModel()
        {
            TransitStateCountryIds = new List<Guid>();
            IntraCountryExportAllowed = new List<Guid>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CountryId.HasValue && CountryId == StateOfExportCountryId)
            {
                // if StateOfExportCompetentAuthority is null, then still need to see if import CA shows up -- if not then still invalid

                if (!IntraCountryExportAllowed.Any(x => CompetentAuthorities == null || CompetentAuthorities.SelectedValue == null ||
                x == CompetentAuthorities.SelectedValue))
                {
                    // not allowed
                    yield return new ValidationResult(StateOfImportResources.ImportExportCountryShouldNotSame, new[] { "CountryId" });
                }
            }
        }
    }
}