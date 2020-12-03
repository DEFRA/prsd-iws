namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetCompetentAuthoritiesAndEntryPointsByCountryId : IRequest<CompetentAuthorityAndEntryOrExitPointData>
    {
        public Guid Id { get; private set; }

        public UKCompetentAuthority NotificationUkCompetentAuthority { get; set; }

        public GetCompetentAuthoritiesAndEntryPointsByCountryId(Guid id, UKCompetentAuthority notificationUkCompetentAuthority)
        {
            Id = id;
            NotificationUkCompetentAuthority = notificationUkCompetentAuthority;
        }
    }
}
