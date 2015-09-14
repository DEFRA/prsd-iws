namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.MovementReceipt;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class UpdateMovementAcceptanceDataByMovementIdHandler : IRequestHandler<UpdateShipmentAcceptanceDataByMovementId, bool>
    {
        private readonly IwsContext context;

        public UpdateMovementAcceptanceDataByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(UpdateShipmentAcceptanceDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            if (message.Decision == Decision.Accepted)
            {
                movement.Receipt.Accept();
            }
            else if (message.Decision == Decision.Rejected)
            {
                movement.Receipt.Reject(message.RejectReason);
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
