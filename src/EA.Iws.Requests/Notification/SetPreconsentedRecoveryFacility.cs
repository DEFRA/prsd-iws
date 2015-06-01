namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class SetPreconsentedRecoveryFacility : IRequest<string>
    {
        public SetPreconsentedRecoveryFacility(Guid notificationId, bool? isPreconsentedRecoveryFacility)
        {
            NotificationId = notificationId;
            IsPreconsentedRecoveryFacility = isPreconsentedRecoveryFacility;
        }

        public Guid NotificationId { get; private set; }

        public bool? IsPreconsentedRecoveryFacility { get; private set; }
    }
}