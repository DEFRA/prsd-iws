namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using EA.Iws.Core.Admin;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditComments)]
    public class GetImportNotificationComments : IRequest<ImportNotificationCommentsData>
    {
        public Guid NotificationId { get; private set; }
        public NotificationShipmentsCommentsType Type { get; private set; }
        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }
        public int ShipmentNumber { get; private set; }
        public int PageNumber { get; private set; }

        public GetImportNotificationComments(Guid notificationId, NotificationShipmentsCommentsType type, int pageNumber, DateTime? startDate, DateTime? endDate, int? shipmentNumber)
        {
            this.NotificationId = notificationId;
            this.Type = type;
            this.PageNumber = pageNumber;
            this.StartDate = startDate == null ? DateTime.MinValue : startDate.GetValueOrDefault();
            this.EndDate = endDate == null ? DateTime.MaxValue : endDate.GetValueOrDefault();
            this.ShipmentNumber = shipmentNumber.GetValueOrDefault();
        }
    }
}
