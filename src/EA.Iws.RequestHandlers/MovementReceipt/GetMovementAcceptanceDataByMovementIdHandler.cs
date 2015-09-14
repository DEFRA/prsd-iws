namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class GetMovementAcceptanceDataByMovementIdHandler : IRequestHandler<GetMovementAcceptanceDataByMovementId, MovementAcceptanceData>
    {
        private readonly IwsContext context;

        public GetMovementAcceptanceDataByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementAcceptanceData> HandleAsync(GetMovementAcceptanceDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            var movementAcceptanceData = new MovementAcceptanceData
            {
                MovementId = message.MovementId,
                RejectionReason = movement.Receipt.RejectReason
            };

            movementAcceptanceData.Decision = movement.Receipt.Decision;

            return movementAcceptanceData;
        }
    }
}
