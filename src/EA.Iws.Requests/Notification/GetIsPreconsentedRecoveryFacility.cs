namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetIsPreconsentedRecoveryFacility : IRequest<bool>
    {
        public Guid NotificationId { get; set; }
    }
}