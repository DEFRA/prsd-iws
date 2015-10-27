namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Facility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Facilities;
    using Core.Shared;
    using Views.Facility;

    public class SiteOfTreatmentViewModel : IValidatableObject
    {
        public SiteOfTreatmentViewModel()
        {
            Facilities = new List<FacilityData>();
        }

        public Guid NotificationId { get; set; }

        public Guid? SelectedSiteOfTreatment { get; set; }

        public IList<FacilityData> Facilities { get; set; }

        public NotificationType NotificationType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SelectedSiteOfTreatment.HasValue)
            {
                var errorMessage = (NotificationType == NotificationType.Recovery)
                    ? "site of recovery"
                    : "site of disposal";

                yield return new ValidationResult(string.Format(SiteOfTreatmentResources.SiteRequired, errorMessage), new[] { "SelectedSiteOfTreatment" });
            }
        }
    }
}