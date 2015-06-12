namespace EA.Iws.Requests.WasteType
{
    using System;
    using Prsd.Core.Mediator;

    public class SetWasteCode : IRequest<Guid>
    {
        public SetWasteCode(Guid wasteCodeId, Guid notificationId)
        {
            NotificationId = notificationId;
            WasteCodeId = wasteCodeId;
        }

        public Guid NotificationId { get; private set; }

        public Guid WasteCodeId { get; private set; }
    }
}