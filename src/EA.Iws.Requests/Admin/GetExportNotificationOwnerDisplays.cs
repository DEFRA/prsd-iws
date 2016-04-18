namespace EA.Iws.Requests.Admin
{
    using System.Collections.Generic;
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationInternal)]
    public class GetExportNotificationOwnerDisplays : IRequest<IEnumerable<ExportNotificationOwnerDisplay>>
    {
    }
}