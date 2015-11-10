namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.PackagingType;
    using Prsd.Core.Mediator;

    public class GetPackagingTypes : IRequest<PackagingData>
    {
        public Guid NotificationId { get; private set; }

        public GetPackagingTypes(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}