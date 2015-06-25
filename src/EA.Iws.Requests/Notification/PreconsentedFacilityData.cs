namespace EA.Iws.Requests.Notification
{
    using System;
    using Shared;

    public class PreconsentedFacilityData
    {
        public Guid NotificationId { get; set; }

        public bool? IsPreconsentedRecoveryFacility { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}