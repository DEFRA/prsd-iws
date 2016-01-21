namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetWorkingDaysUntil : IRequest<int>
    {
        public DateTime Date { get; private set; }

        public Guid NotificationId { get; private set; }

        public GetWorkingDaysUntil(Guid notificationId, DateTime date)
        {
            Date = date;
            NotificationId = notificationId;
        }
    }
}
