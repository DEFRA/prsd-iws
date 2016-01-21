namespace EA.Iws.Requests.Shared
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetCountry : IRequest<CountryData>
    {
        public Guid CountryId { get; set; }
    }
}