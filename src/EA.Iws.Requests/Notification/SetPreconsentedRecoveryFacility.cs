namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class SetPreconsentedRecoveryFacility : IRequest<string>
    {
        public SetPreconsentedRecoveryFacility(Guid notificationId, bool? isPreconsented)
        {
            NotificationId = notificationId;
            IsPreconsented = isPreconsented;
        }

        public Guid NotificationId { get; private set; }

        public bool? IsPreconsented { get; private set; }
    }
}