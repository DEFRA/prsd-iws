namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;
    using Prsd.Core.Mediator;
    public class SetOptionalWasteCodes : IRequest<Guid>
    {
        public SetOptionalWasteCodes(Guid notificationId, List<WasteCodeData> optionalWasteCodes)
        {
            NotificationId = notificationId;
            OptionalWasteCodes = optionalWasteCodes;
        }
        public Guid NotificationId { get; private set; }
        public List<WasteCodeData> OptionalWasteCodes { get; private set; }
    }
}