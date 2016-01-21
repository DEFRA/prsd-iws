namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.TransportRoute;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetEntryOrExitPointsByCountry : IRequest<IList<EntryOrExitPointData>>
    {
        public Guid CountryId { get; private set; }

        public GetEntryOrExitPointsByCountry(Guid countryId)
        {
            CountryId = countryId;
        }
    }
}