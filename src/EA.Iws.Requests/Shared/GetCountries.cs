namespace EA.Iws.Requests.Shared
{
    using System.Collections.Generic;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;
    using Registration;

    [AllowUnauthorizedUser]
    public class GetCountries : IRequest<List<CountryData>>
    {
    }
}