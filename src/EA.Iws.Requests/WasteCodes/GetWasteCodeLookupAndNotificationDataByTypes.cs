namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    public class GetWasteCodeLookupAndNotificationDataByTypes : IRequest<WasteCodeDataAndNotificationData>
    {
        public Guid Id { get; private set; }

        public IList<CodeType> LookupWasteCodeTypes { get; private set; }

        public IList<CodeType> NotificationWasteCodeTypes { get; private set; }

        public GetWasteCodeLookupAndNotificationDataByTypes(Guid id, IList<CodeType> lookupWasteCodeTypes = null, IList<CodeType> notificationWasteCodeTypes = null)
        {
            Id = id;
            LookupWasteCodeTypes = lookupWasteCodeTypes ?? new List<CodeType>();
            NotificationWasteCodeTypes = notificationWasteCodeTypes ?? new List<CodeType>();
        }
    }
}
