namespace EA.Iws.Web.ViewModels.Facility
{
    using System;
    using System.Collections.Generic;
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