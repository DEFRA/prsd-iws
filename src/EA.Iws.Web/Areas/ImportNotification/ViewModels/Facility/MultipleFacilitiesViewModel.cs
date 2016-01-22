namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Facility
{
    using System;
    using System.Collections.Generic;
    using Core.ImportNotification.Draft;
    using Core.Shared;

    public class MultipleFacilitiesViewModel
    {
        public MultipleFacilitiesViewModel()
        {
            Facilities = new List<Facility>();
        }

        public Guid NotificationId { get; set; }

        public IList<Facility> Facilities { get; set; }

        public NotificationType NotificationType { get; set; }

        public Guid SelectedSiteOfTreatment { get; set; }
    }
}