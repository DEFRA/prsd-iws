namespace EA.Iws.Requests.Facilities
{
    using System;

    [NotificationReadOnlyAuthorize]
    public class UpdateFacilityForNotification : AddFacilityToNotification
    {
        public Guid FacilityId { get; set; }
    }
}