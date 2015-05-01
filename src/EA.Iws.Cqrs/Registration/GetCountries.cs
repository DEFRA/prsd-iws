namespace EA.Iws.Cqrs.Registration
{
    using System.Collections.Generic;
    using Api.Client.Entities;
    using Core.Cqrs;

    public class GetCountries : IQuery<CountryData[]>
    {
    }
}
