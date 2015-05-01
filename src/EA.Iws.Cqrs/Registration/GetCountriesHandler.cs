namespace EA.Iws.Cqrs.Registration
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Client.Entities;
    using Core.Cqrs;
    using DataAccess;

    internal class GetCountriesHandler : IQueryHandler<GetCountries, CountryData[]>
    {
        private readonly IwsContext context;

        public GetCountriesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<CountryData[]> ExecuteAsync(GetCountries query)
        {
            var result = await context.Countries.ToArrayAsync();
            var countryData = result.Select(c => new CountryData()
            {
                Name = c.Name,
                Id = c.Id,
            }).OrderBy(c => c.Name).ToArray();
            return countryData;
        }
    }
}