namespace EA.Iws.RequestHandlers.Shared
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Shared;

    internal class GetEuropeanUnionCountriesHandler : IRequestHandler<GetEuropeanUnionCountries, CountryData[]>
    {
        private readonly IwsContext context;
        private readonly IMap<Country, CountryData> mapper;

        public GetEuropeanUnionCountriesHandler(IwsContext context, IMap<Country, CountryData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CountryData[]> HandleAsync(GetEuropeanUnionCountries message)
        {
            var countries = await context.Countries.Where(c => c.IsEuropeanUnionMember).ToArrayAsync();

            return countries.Select(mapper.Map).OrderByDescending(c => c.Name).ToArray();
        }
    }
}
