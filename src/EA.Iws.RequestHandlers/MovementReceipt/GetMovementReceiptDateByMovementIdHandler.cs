namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class GetMovementReceiptDateByMovementIdHandler : IRequestHandler<GetMovementReceiptDateByMovementId, MovementReceiptDateData>
    {
        private readonly IwsContext context;

        public GetMovementReceiptDateByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementReceiptDateData> HandleAsync(GetMovementReceiptDateByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            var dates = new MovementReceiptDateData { MovementDate = movement.Date.Value };

            if (movement.Receipt != null)
            {
                dates.DateReceived = movement.Receipt.Date;
            }

            return dates;
        }
    }
}
