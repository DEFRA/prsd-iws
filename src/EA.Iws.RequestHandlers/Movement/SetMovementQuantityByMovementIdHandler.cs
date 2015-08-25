namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SetMovementQuantityByMovementIdHandler : IRequestHandler<SetMovementQuantityByMovementId, bool>
    {
        private readonly IwsContext context;

        public SetMovementQuantityByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetMovementQuantityByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.Id);

            movement.SetQuantity(message.Quantity, message.Units);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
