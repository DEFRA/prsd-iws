namespace EA.Iws.Requests.WasteType
{
    using System;
    using Prsd.Core.Mediator;

    public class SetBaselOrOecdWasteCode : IRequest<Guid>
    {
        public SetBaselOrOecdWasteCode(Guid wasteCodeId, Guid notificationId)
        {
            NotificationId = notificationId;
            WasteCodeId = wasteCodeId;
        }

        public Guid NotificationId { get; private set; }

        public Guid WasteCodeId { get; private set; }
    }
}