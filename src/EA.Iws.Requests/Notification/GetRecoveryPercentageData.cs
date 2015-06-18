namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetRecoveryPercentageData : IRequest<RecoveryPercentageData>
    {
        public GetRecoveryPercentageData(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
