namespace EA.Iws.Requests.Notification
{
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetNotificationsByUser : IRequest<IList<NotificationApplicationSummaryData>>
    {
    }
}