namespace EA.Iws.RequestHandlers.Movement.Receive
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;

    internal class DoesQuantityReceivedExceedToleranceHandler : IRequestHandler<DoesQuantityReceivedExceedTolerance, QuantityReceivedTolerance>
    {
        private readonly IMovementDetailsRepository movementDetailsRepository;

        public DoesQuantityReceivedExceedToleranceHandler(IMovementDetailsRepository movementDetailsRepository)
        {
            this.movementDetailsRepository = movementDetailsRepository;
        }

        public async Task<QuantityReceivedTolerance> HandleAsync(DoesQuantityReceivedExceedTolerance message)
        {
            var movementDetails = await movementDetailsRepository.GetByMovementId(message.MovementId);

            var shipmentQuantity = new ShipmentQuantity(message.Quantity, message.Units);

            var intendedQuantity = movementDetails.ActualQuantity.Quantity;
            var units = movementDetails.ActualQuantity.Units;

            if (shipmentQuantity < new ShipmentQuantity(intendedQuantity * 0.5m, units))
            {
                return QuantityReceivedTolerance.BelowTolerance;
            }

            if (shipmentQuantity > new ShipmentQuantity(intendedQuantity * 1.5m, units))
            {
                return QuantityReceivedTolerance.AboveTolerance;
            }

            return QuantityReceivedTolerance.WithinTolerance;
        }
    }
}
