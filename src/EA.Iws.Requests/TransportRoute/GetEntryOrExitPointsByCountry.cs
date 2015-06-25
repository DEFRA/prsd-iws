namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using System.Collections.Generic;
    using Core.TransportRoute;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetEntryOrExitPointsByCountry : IRequest<IList<EntryOrExitPointData>>
    {
        public Guid CountryId { get; private set; }

        public GetEntryOrExitPointsByCountry(Guid countryId)
        {
            CountryId = countryId;
        }
    }
}
