namespace EA.Iws.Requests.Shared
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetCompetentAuthoritiesByCountry : IRequest<ICollection<CompetentAuthorityData>>
    {
        public Guid CountryId { get; private set; }

        public GetCompetentAuthoritiesByCountry(Guid countryId)
        {
            this.CountryId = countryId;
        }
    }
}
