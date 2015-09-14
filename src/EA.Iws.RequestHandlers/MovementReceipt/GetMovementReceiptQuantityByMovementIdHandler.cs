namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.MovementReceipt;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class GetMovementReceiptQuantityByMovementIdHandler : IRequestHandler<GetMovementReceiptQuantityByMovementId, MovementReceiptQuantityData>
    {
        private readonly IwsContext context;

        public GetMovementReceiptQuantityByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementReceiptQuantityData> HandleAsync(GetMovementReceiptQuantityByMovementId message)
        {
            var movement = await context.Movements.Where(m => m.Id == message.Id)
                .Select(m => new
                {
                    m.DisplayUnits,
                    m.Receipt,
                    m.Units
                })
                .SingleAsync();

            if (movement.Receipt == null)
            {
                return new MovementReceiptQuantityData
                {
                    MovementUnit = movement.DisplayUnits.Value
                };
            }

            return new MovementReceiptQuantityData
            {
                MovementUnit = movement.DisplayUnits.Value,
                Quantity = (movement.Receipt.Quantity.HasValue) ? (decimal?)ShipmentQuantityUnitConverter.ConvertToTarget(movement.Units.Value,
                    movement.DisplayUnits.Value,
                    movement.Receipt.Quantity.Value) : null
            };
        }
    }
}
