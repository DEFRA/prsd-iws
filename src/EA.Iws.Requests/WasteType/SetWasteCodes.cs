namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    public class SetWasteCodes : IRequest<Guid>
    {
        public SetWasteCodes(IEnumerable<WasteCodeData> wasteCodes, Guid notificationId)
        {
            NotificationId = notificationId;
            WasteCodes = wasteCodes;
        }

        public Guid NotificationId { get; private set; }

        public IEnumerable<WasteCodeData> WasteCodes { get; private set; }
    }
}