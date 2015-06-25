namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.TransportRoute;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TransportRoute;

    internal class GetEntryOrExitPointsByCountryHandler : IRequestHandler<GetEntryOrExitPointsByCountry, IList<EntryOrExitPointData>>
    {
        private readonly IwsContext context;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> mapper;

        public GetEntryOrExitPointsByCountryHandler(IwsContext context, IMap<EntryOrExitPoint, EntryOrExitPointData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IList<EntryOrExitPointData>> HandleAsync(GetEntryOrExitPointsByCountry message)
        {
            var entryOrExitPoints = await context.EntryOrExitPoints.Where(ep => ep.Country.Id == message.CountryId).ToArrayAsync();

            return entryOrExitPoints.Select(mapper.Map).ToArray();
        }
    }
}
