namespace EA.Iws.Requests.PackagingType
{
    using System;
    using Prsd.Core.Mediator;

    public class GetPackagingInfoForNotification : IRequest<PackagingData>
    {
        public GetPackagingInfoForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}