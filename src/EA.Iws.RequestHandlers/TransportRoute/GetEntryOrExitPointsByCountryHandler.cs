namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.TransportRoute;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TransportRoute;

    internal class GetEntryOrExitPointsByCountryHandler : IRequestHandler<GetEntryOrExitPointsByCountry, IList<EntryOrExitPointData>>
    {
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> mapper;

        public GetEntryOrExitPointsByCountryHandler(IEntryOrExitPointRepository entryOrExitPointRepository, IMap<EntryOrExitPoint, EntryOrExitPointData> mapper)
        {
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.mapper = mapper;
        }

        public async Task<IList<EntryOrExitPointData>> HandleAsync(GetEntryOrExitPointsByCountry message)
        {
            var entryOrExitPoints = await entryOrExitPointRepository.GetForCountry(message.CountryId);

            return entryOrExitPoints.Select(mapper.Map).ToArray();
        }
    }
}
