namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class SetEwcCodes : IRequest<Guid>
    {
        public SetEwcCodes(IEnumerable<WasteCodeData> ewcWasteCodes, Guid notificationId)
        {
            NotificationId = notificationId;
            EwcWasteCodes = ewcWasteCodes;
        }

        public Guid NotificationId { get; private set; }

        public IEnumerable<WasteCodeData> EwcWasteCodes { get; private set; }
    }
}