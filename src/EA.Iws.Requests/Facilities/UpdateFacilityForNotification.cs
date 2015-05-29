namespace EA.Iws.Requests.Facilities
{
    using System;

    public class UpdateFacilityForNotification : AddFacilityToNotification
    {
        public Guid FacilityId { get; set; }
    }
}