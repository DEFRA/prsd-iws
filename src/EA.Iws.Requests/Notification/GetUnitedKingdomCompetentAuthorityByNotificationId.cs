namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetUnitedKingdomCompetentAuthorityByNotificationId : IRequest<UnitedKingdomCompetentAuthorityData>
    {
        public Guid Id { get; private set; }

        public GetUnitedKingdomCompetentAuthorityByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
