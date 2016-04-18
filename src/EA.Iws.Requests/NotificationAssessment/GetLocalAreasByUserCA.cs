namespace EA.Iws.Requests.NotificationAssessment
{
    using System.Collections.Generic;
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetLocalAreasByUserCa : IRequest<List<LocalAreaData>>
    {
    }
}
