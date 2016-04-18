namespace EA.Iws.RequestHandlers.Admin.EntryOrExitPoints
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.TransportRoute;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.EntryOrExitPoints;

    public class GetEntryOrExitPointsGroupedByCountryHandler : IRequestHandler<GetEntryOrExitPointsGroupedByCountry, EntryOrExitPointDataGroup[]>
    {
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IMapper mapper;

        public GetEntryOrExitPointsGroupedByCountryHandler(IEntryOrExitPointRepository entryOrExitPointRepository,
            IMapper mapper)
        {
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.mapper = mapper;
        }

        public async Task<EntryOrExitPointDataGroup[]> HandleAsync(GetEntryOrExitPointsGroupedByCountry message)
        {
            var entryOrExitPoints = await entryOrExitPointRepository.GetAll();

            var returnData = entryOrExitPoints.GroupBy(e => e.Country)
                .Select(g => new EntryOrExitPointDataGroup
                {
                    Country = mapper.Map<CountryData>(g.Key),
                    EntryOrExitPoints = g.Select(e => mapper.Map<EntryOrExitPointData>(e)).OrderBy(e => e.Name)
                        .ToArray()
                }).OrderBy(r => r.Country.Name).ToArray();

            return returnData;
        }
    }
}
