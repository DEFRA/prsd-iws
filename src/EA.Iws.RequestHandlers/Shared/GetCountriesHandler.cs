namespace EA.Iws.RequestHandlers.Shared
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Shared;

    internal class GetCountriesHandler : IRequestHandler<GetCountries, List<CountryData>>
    {
        private readonly IwsContext context;

        public GetCountriesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<List<CountryData>> HandleAsync(GetCountries query)
        {
            var result = await context.Countries.ToArrayAsync();
            var countryData = result.Select(c => new CountryData
            {
                Name = c.Name,
                Id = c.Id
            }).OrderBy(c => c.Name).ToArray();
            return countryData.ToList();
        }
    }
}