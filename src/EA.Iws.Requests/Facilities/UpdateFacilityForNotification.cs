namespace EA.Iws.Requests.Facilities
{
    using System;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class UpdateFacilityForNotification : AddFacilityToNotification
    {
        public Guid FacilityId { get; set; }
    }
}