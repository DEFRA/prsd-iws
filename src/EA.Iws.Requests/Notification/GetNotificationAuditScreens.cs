namespace EA.Iws.Requests.Notification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification.Audit;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetNotificationAuditScreens : IRequest<IList<NotificationAuditScreen>>
    {
        public GetNotificationAuditScreens()
        {
        }
    }
}
