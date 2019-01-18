namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetNotificationAuditTable : IRequest<NotificationAuditTable>
    {
        public Guid NotificationId { get; private set; }

        public int PageNumber { get; private set; }

        public int Screen { get;  private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public GetNotificationAuditTable(Guid notificationId, int pageNumber, int screen, DateTime? startDate, DateTime? endDate)
        {
            this.NotificationId = notificationId;
            this.PageNumber = pageNumber;
            this.Screen = screen;
            this.StartDate = startDate == null ? DateTime.MinValue : startDate.GetValueOrDefault();
            this.EndDate = endDate == null ? DateTime.MaxValue : endDate.GetValueOrDefault();
        }
    }
}
