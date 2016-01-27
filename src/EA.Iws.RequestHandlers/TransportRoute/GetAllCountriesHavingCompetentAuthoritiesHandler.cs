namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TransportRoute;

    internal class GetAllCountriesHavingCompetentAuthoritiesHandler : IRequestHandler<GetAllCountriesHavingCompetentAuthorities, CountryData[]>
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public GetAllCountriesHavingCompetentAuthoritiesHandler(ICountryRepository countryRepository, IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        public async Task<CountryData[]> HandleAsync(GetAllCountriesHavingCompetentAuthorities message)
        {
            return (await countryRepository.GetAllHavingCompetentAuthorities())
                .Select(c => mapper.Map<CountryData>(c))
                .ToArray();
        }
    }
}
