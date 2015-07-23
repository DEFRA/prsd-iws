namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Facility
{
    using System;
    using Core.Shared;

    public class RemoveFacilityViewModel
    {
        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public Guid FacilityId { get; set; }

        public string FacilityName { get; set; }
    }
}