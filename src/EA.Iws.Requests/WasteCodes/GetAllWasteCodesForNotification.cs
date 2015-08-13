namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    public class GetAllWasteCodesForNotification : IRequest<WasteCodeData[]>
    {
        public GetAllWasteCodesForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}