namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    public class GetWasteCodesForNotification : IRequest<WasteCodeData[]>
    {
        public GetWasteCodesForNotification(Guid notificationId, CodeType codeType)
        {
            NotificationId = notificationId;
            CodeType = codeType;
        }

        public Guid NotificationId { get; private set; }

        public CodeType CodeType { get; private set; }
    }
}