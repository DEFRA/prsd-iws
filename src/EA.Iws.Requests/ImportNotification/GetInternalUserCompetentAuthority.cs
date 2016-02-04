namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetInternalUserCompetentAuthority : IRequest<Tuple<UKCompetentAuthority, CompetentAuthorityData>>
    {
    }
}
