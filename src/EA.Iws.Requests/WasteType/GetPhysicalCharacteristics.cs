namespace EA.Iws.Requests.WasteType
{
    using System;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    public class GetPhysicalCharacteristics : IRequest<PhysicalCharacteristicsData>
    {
        public GetPhysicalCharacteristics(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}