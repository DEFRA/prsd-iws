namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetNumberOfPackagesByMovementIdHandler : IRequestHandler<GetNumberOfPackagesByMovementId, int?>
    {
        private readonly IwsContext context;

        public GetNumberOfPackagesByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<int?> HandleAsync(GetNumberOfPackagesByMovementId message)
        {
            return await context.Movements
                .Where(m => m.Id == message.Id)
                .Select(m => m.NumberOfPackages)
                .SingleAsync();
        }
    }
}
