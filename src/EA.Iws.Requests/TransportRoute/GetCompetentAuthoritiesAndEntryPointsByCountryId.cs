namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetCompetentAuthoritiesAndEntryPointsByCountryId : IRequest<CompetentAuthorityAndEntryOrExitPointData>
    {
        public Guid Id { get; private set; }
        public Guid? ExitPointCompetentAuthorityId { get; private set; }

        public GetCompetentAuthoritiesAndEntryPointsByCountryId(Guid id, Guid? exitPointCompetentAuthorityId)
        {
            Id = id;
            ExitPointCompetentAuthorityId = exitPointCompetentAuthorityId;
        }
    }
}
