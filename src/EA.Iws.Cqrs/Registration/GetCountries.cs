namespace EA.Iws.Cqrs.Registration
{
    using Prsd.Core.Mediator;

    public class GetCountries : IRequest<CountryData[]>
    {
    }
}