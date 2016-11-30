namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanChangeNumberOfShipmentsOnExportNotification)]
    public class GetChangeNumberOfShipmentConfrimationData : IRequest<ConfirmNumberOfShipmentsChangeData>
    {
        public GetChangeNumberOfShipmentConfrimationData(Guid notificationId, int numberOfShipments)
        {
            NotificationId = notificationId;
            NumberOfShipments = numberOfShipments;
        }

        public Guid NotificationId { get; private set; }

        public int NumberOfShipments { get; private set; }
    }
}
