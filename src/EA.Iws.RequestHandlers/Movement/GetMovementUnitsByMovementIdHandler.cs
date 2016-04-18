namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementUnitsByMovementIdHandler : IRequestHandler<GetMovementUnitsByMovementId, ShipmentQuantityUnits>
    {
        private readonly IMovementDetailsRepository repository;

        public GetMovementUnitsByMovementIdHandler(IMovementDetailsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ShipmentQuantityUnits> HandleAsync(GetMovementUnitsByMovementId message)
        {
            var movementDetails = await repository.GetByMovementId(message.Id);

            return movementDetails.ActualQuantity.Units;
        }
    }
}