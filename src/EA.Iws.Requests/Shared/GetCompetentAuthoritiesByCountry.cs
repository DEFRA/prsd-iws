namespace EA.Iws.Requests.Shared
{
    using System;
    using System.Collections.Generic;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetCompetentAuthoritiesByCountry : IRequest<ICollection<CompetentAuthorityData>>
    {
        public Guid CountryId { get; private set; }

        public GetCompetentAuthoritiesByCountry(Guid countryId)
        {
            this.CountryId = countryId;
        }
    }
}
