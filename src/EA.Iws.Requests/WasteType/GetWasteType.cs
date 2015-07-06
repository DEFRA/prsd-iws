namespace EA.Iws.Requests.WasteType
{
    using System;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    public class GetWasteType : IRequest<WasteTypeData>
    {
        public GetWasteType(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}