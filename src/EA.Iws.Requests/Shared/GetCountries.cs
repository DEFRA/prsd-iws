﻿namespace EA.Iws.Requests.Shared
{
    using System.Collections.Generic;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetCountries : IRequest<List<CountryData>>
    {
    }
}