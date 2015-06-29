namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class SetWasteCodes : IRequest<Guid>
    {
        public SetWasteCodes(Guid notificationId, Guid basedOecdCode, IEnumerable<Guid> ewcCodes)
        {
            NotificationId = notificationId;
            BasedOecdCode = basedOecdCode;
            EwcCodes = ewcCodes;
        }

        public Guid BasedOecdCode { get; private set; }

        public Guid NotificationId { get; private set; }

        public IEnumerable<Guid> EwcCodes { get; private set; }
    }
}