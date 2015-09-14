namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class GetMovementReceiptDateByMovementIdHandler : IRequestHandler<GetMovementReceiptDateByMovementId, DateTime?>
    {
        private readonly IwsContext context;

        public GetMovementReceiptDateByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<DateTime?> HandleAsync(GetMovementReceiptDateByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            if (movement.Receipt != null)
            {
                return movement.Receipt.Date;
            }
            else
            {
                return null;
            }
        }
    }
}
