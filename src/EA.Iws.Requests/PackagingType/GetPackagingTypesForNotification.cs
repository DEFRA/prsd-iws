namespace EA.Iws.Requests.PackagingType
{
    using System;
    using Prsd.Core.Mediator;

    public class GetPackagingTypesForNotification : IRequest<PackagingData>
    {
        public GetPackagingTypesForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}