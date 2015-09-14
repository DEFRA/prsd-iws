namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class CreateMovementReceiptForMovementHandler : IRequestHandler<CreateMovementReceiptForMovement, bool>
    {
        private readonly IwsContext context;

        public CreateMovementReceiptForMovementHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(CreateMovementReceiptForMovement message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            movement.Receive(message.DateReceived);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
