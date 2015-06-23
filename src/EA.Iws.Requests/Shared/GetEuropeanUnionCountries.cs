namespace EA.Iws.Requests.Shared
{
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetEuropeanUnionCountries : IRequest<CountryData[]>
    {
    }
}
