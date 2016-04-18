namespace EA.Iws.Requests.NotificationAssessment
{
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetNotificationAttentionSummary : IRequest<IEnumerable<NotificationAttentionSummaryTableData>>
    {
    }
}