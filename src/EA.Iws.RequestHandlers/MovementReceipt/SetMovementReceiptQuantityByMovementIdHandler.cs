namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class SetMovementReceiptQuantityByMovementIdHandler : IRequestHandler<SetMovementReceiptQuantityByMovementId, bool>
    {
        private readonly IwsContext context;

        public SetMovementReceiptQuantityByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetMovementReceiptQuantityByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.Id);

            movement.Receipt.SetQuantity(message.Quantity, message.Units, movement.Units.Value);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
