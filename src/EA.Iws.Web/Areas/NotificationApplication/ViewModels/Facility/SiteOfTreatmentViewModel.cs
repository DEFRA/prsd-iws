namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Facility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Facilities;
    using Core.Shared;

    public class SiteOfTreatmentViewModel
    {
        public SiteOfTreatmentViewModel()
        {
            Facilities = new List<FacilityData>();
        }

        public Guid NotificationId { get; set; }

        [Required]
        [Display(Name = "Site of treatment")]
        public Guid? SelectedSiteOfTreatment { get; set; }

        public IList<FacilityData> Facilities { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}