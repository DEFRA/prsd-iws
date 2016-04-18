namespace EA.Iws.Requests.Shared
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetEuropeanUnionCountries : IRequest<CountryData[]>
    {
    }
}