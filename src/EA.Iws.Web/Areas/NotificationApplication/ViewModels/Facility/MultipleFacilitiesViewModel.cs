namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Facility
{
    using System;
    using System.Collections.Generic;
    using Core.Facilities;
    using Core.Shared;
    using Requests.Facilities;
    using Requests.Shared;

    public class MultipleFacilitiesViewModel
    {
        public MultipleFacilitiesViewModel()
        {
            FacilityData = new List<FacilityData>();
        }

        public Guid NotificationId { get; set; }

        public List<FacilityData> FacilityData { get; set; }

        public NotificationType NotificationType { get; set; }

        public string SelectedValue { get; set; }
    }
}