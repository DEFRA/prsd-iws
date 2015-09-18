namespace EA.Iws.RequestHandlers.MovementOperationReceipt
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MovementOperationReceipt;

    public class CreateMovementOperationReceiptForMovementHandler : IRequestHandler<CreateMovementOperationReceiptForMovement, bool>
    {
        private readonly IwsContext context;

        public CreateMovementOperationReceiptForMovementHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(CreateMovementOperationReceiptForMovement message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            movement.CompleteMovement(message.DateComplete);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
