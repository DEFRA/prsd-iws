namespace EA.Iws.Requests.Shared
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetCountry : IRequest<CountryData>
    {
        public Guid CountryId { get; set; }
    }
}
