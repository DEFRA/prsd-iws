namespace EA.Iws.Cqrs.Registration
{
    using Api.Client.Entities;
    using Core.Cqrs;

    public class GetCountries : IQuery<CountryData[]>
    {
    }
}
