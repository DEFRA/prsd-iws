namespace EA.Iws.Requests.Notification
{
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanGetNotificationsForApplicantHome)]
    public class GetExportNotificationsByUser : IRequest<IList<NotificationApplicationSummaryData>>
    {
    }
}