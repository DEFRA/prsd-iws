namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using EA.Iws.Core.Admin;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditInternalComments)]
    public class GetNotificationComments : IRequest<NotificationCommentData>
    {
        public Guid NotificationId { get; private set; }
        public NotificationShipmentsCommentsType Type { get; private set; }
        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }
        public int ShipmentNumber { get; private set; }

        public GetNotificationComments(Guid notificationId, NotificationShipmentsCommentsType type, DateTime? startDate, DateTime? endDate, int? shipmentNumber)
        {
            this.NotificationId = notificationId;
            this.Type = type;
            this.StartDate = startDate == null ? DateTime.MinValue : startDate.GetValueOrDefault();
            this.EndDate = endDate == null ? DateTime.MaxValue : endDate.GetValueOrDefault();
            this.ShipmentNumber = shipmentNumber.GetValueOrDefault();
        }
    }
}
