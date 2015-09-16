namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class GetMovementReceiptSummaryDataByMovementIdHandler : IRequestHandler<GetMovementReceiptSummaryDataByMovementId, MovementReceiptSummaryData>
    {
        private readonly IwsContext context;

        public GetMovementReceiptSummaryDataByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementReceiptSummaryData> HandleAsync(GetMovementReceiptSummaryDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.Id);
            
            var financialGuarantee = await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == movement.NotificationApplicationId);

            var totalActiveMovements = await context.Movements.CountAsync(m => m.NotificationApplicationId == movement.NotificationApplicationId
                && m.Date.HasValue
                && m.Date < SystemTime.UtcNow);

            var quantityReceived = await context.Movements.Where(m => 
                        m.Receipt != null
                        && m.Receipt.Decision == Core.MovementReceipt.Decision.Accepted
                        && m.Receipt.Quantity.HasValue)
                    .Select(m => m.Receipt.Quantity)
                    .SumAsync();

            var quantityRemaining = movement.NotificationApplication.ShipmentInfo.Quantity - quantityReceived.GetValueOrDefault();

            var data = new MovementReceiptSummaryData
            {
                NotificationId = movement.NotificationApplicationId,
                MovementId = movement.Id,
                NotificationNumber = movement.NotificationApplication.NotificationNumber,
                ThisMovementNumber = movement.Number,
                ActiveLoadsPermitted = financialGuarantee.ActiveLoadsPermitted.GetValueOrDefault(),
                CurrentActiveLoads = totalActiveMovements,
                QuantitySoFar = quantityReceived.GetValueOrDefault(),
                QuantityRemaining = quantityRemaining,
                DisplayUnit = movement.NotificationApplication.ShipmentInfo.Units
            };

            return data;
        }
    }
}
