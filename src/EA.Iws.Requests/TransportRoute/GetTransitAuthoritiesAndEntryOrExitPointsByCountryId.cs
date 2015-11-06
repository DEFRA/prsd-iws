namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Prsd.Core.Mediator;

    public class GetTransitAuthoritiesAndEntryOrExitPointsByCountryId : IRequest<CompetentAuthorityAndEntryOrExitPointData>
    {
        public Guid Id { get; private set; }

        public GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(Guid id)
        {
            Id = id;
        }
    }
}
