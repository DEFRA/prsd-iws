namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SetNumberOfPackagesByMovementIdHandler : IRequestHandler<SetNumberOfPackagesByMovementId, bool>
    {
        private readonly IwsContext context;

        public SetNumberOfPackagesByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetNumberOfPackagesByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.Id);

            movement.SetNumberOfPackages(message.NumberOfPackages);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
