namespace EA.Iws.RequestHandlers.ImportNotification.TransportRoute
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.TransitState;
    using Core.TransportRoute;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.TransportRoute;

    internal class GetTransitStateDataForTransitStatesHandler : IRequestHandler<GetTransitStateDataForTransitStates, IList<TransitStateData>>
    {
        private readonly ICountryRepository countryRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IMapper mapper;

        public GetTransitStateDataForTransitStatesHandler(ICountryRepository countryRepository, 
            ICompetentAuthorityRepository competentAuthorityRepository, 
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.mapper = mapper;
        }

        public async Task<IList<TransitStateData>> HandleAsync(GetTransitStateDataForTransitStates message)
        {
            var transitStates = new List<TransitStateData>();

            foreach (var transitState in message.TransitStates)
            {
                transitStates.Add(await GetTransitStateData(transitState));
            }

            return transitStates;
        }

        private async Task<TransitStateData> GetTransitStateData(Core.ImportNotification.Draft.TransitState transitState)
        {
            var transitStateData = new TransitStateData
            {
                Id = transitState.Id,
                OrdinalPosition = transitState.OrdinalPosition
            };

            if (transitState.CountryId.HasValue)
            {
                var country = await countryRepository.GetById(transitState.CountryId.Value);
                transitStateData.Country = mapper.Map<CountryData>(country);
            }

            if (transitState.CompetentAuthorityId.HasValue)
            {
                var competentAuthority =
                    await competentAuthorityRepository.GetById(transitState.CompetentAuthorityId.Value);
                transitStateData.CompetentAuthority = mapper.Map<CompetentAuthorityData>(competentAuthority);
            }

            if (transitState.EntryPointId.HasValue)
            {
                var entryPoint = await entryOrExitPointRepository.GetById(transitState.EntryPointId.Value);
                transitStateData.EntryPoint = mapper.Map<EntryOrExitPointData>(entryPoint);
            }

            if (transitState.ExitPointId.HasValue)
            {
                var exitPoint = await entryOrExitPointRepository.GetById(transitState.ExitPointId.Value);
                transitStateData.ExitPoint = mapper.Map<EntryOrExitPointData>(exitPoint);
            }

            return transitStateData;
        } 
    }
}
