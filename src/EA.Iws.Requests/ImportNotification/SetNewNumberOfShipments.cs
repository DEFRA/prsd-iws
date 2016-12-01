namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanChangeNumberOfShipmentsOnImportNotification)]
    public class SetNewNumberOfShipments : IRequest<bool>
    {
        public SetNewNumberOfShipments(Guid notificationId, int oldNumberOfShipments, int newNumberOfShipments)
        {
            NotificationId = notificationId;
            NewNumberOfShipments = newNumberOfShipments;
            OldNumberOfShipments = oldNumberOfShipments;
        }

        public Guid NotificationId { get; private set; }

        public int NewNumberOfShipments { get; private set; }

        public int OldNumberOfShipments { get; private set; }
    }
}
