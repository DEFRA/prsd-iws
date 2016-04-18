namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId : IRequest<CompetentAuthorityAndEntryOrExitPointData>
    {
        public Guid Id { get; private set; }

        public GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(Guid id)
        {
            Id = id;
        }
    }
}
