namespace EA.Iws.RequestHandlers.Admin.EntryOrExitPoints
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.Admin.EntryOrExitPoints;

    internal class CheckEntryOrExitPointUniqueHandler : IRequestHandler<CheckEntryOrExitPointUnique, bool>
    {
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;

        public CheckEntryOrExitPointUniqueHandler(IEntryOrExitPointRepository entryOrExitPointRepository)
        {
            this.entryOrExitPointRepository = entryOrExitPointRepository;
        }

        public async Task<bool> HandleAsync(CheckEntryOrExitPointUnique message)
        {
            var entryOrExitPoints = await entryOrExitPointRepository.GetForCountry(message.CountryId);

            return
                !entryOrExitPoints.Any(eep => eep.Name.Equals(message.Name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
