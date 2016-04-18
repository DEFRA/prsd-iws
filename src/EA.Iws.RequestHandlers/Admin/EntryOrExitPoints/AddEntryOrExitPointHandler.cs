namespace EA.Iws.RequestHandlers.Admin.EntryOrExitPoints
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.Admin.EntryOrExitPoints;

    internal class AddEntryOrExitPointHandler : IRequestHandler<AddEntryOrExitPoint, bool>
    {
        private readonly IwsContext context;
        private readonly ICountryRepository countryRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;

        public AddEntryOrExitPointHandler(IwsContext context, 
            ICountryRepository countryRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository)
        {
            this.context = context;
            this.countryRepository = countryRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
        }

        public async Task<bool> HandleAsync(AddEntryOrExitPoint message)
        {
            var country = await countryRepository.GetById(message.CountryId);

            await entryOrExitPointRepository.Add(new EntryOrExitPoint(message.Name, country));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
