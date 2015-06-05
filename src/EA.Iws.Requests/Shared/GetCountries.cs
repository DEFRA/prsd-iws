namespace EA.Iws.Requests.Shared
{
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;
    using Registration;

    [AllowUnauthorizedUser]
    public class GetCountries : IRequest<CountryData[]>
    {
    }
}