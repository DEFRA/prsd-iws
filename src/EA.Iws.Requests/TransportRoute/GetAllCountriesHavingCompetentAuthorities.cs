namespace EA.Iws.Requests.TransportRoute
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetAllCountriesHavingCompetentAuthorities : IRequest<CountryData[]>
    {
    }
}
