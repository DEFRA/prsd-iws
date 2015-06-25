namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetIsPreconsentedRecoveryFacility : IRequest<PreconsentedFacilityData>
    {
        public GetIsPreconsentedRecoveryFacility(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}