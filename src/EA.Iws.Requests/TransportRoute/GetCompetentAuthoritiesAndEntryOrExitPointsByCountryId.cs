namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Prsd.Core.Mediator;

    public class GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId : IRequest<CompententAuthorityAndEntryOrExitPointData>
    {
        public Guid Id { get; private set; }

        public GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(Guid id)
        {
            Id = id;
        }
    }
}
