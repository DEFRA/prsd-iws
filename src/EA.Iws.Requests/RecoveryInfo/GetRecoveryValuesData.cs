namespace EA.Iws.Requests.RecoveryInfo
{
    using System;
    using Core.RecoveryInfo;
    using Prsd.Core.Mediator;

    public class GetRecoveryValuesData : IRequest<RecoveryInfoData>
    {
        public Guid NotificationId { get; private set; }

        public GetRecoveryValuesData(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
