namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementUnitsByMovementIdHandler : IRequestHandler<GetMovementUnitsByMovementId, ShipmentQuantityUnits>
    {
        private readonly IwsContext context;

        public GetMovementUnitsByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ShipmentQuantityUnits> HandleAsync(GetMovementUnitsByMovementId message)
        {
            var movement = await context.Movements.Where(m => m.Id == message.Id)
                .Select(m => new
                {
                    m.Units
                })
                .SingleAsync();

            return movement.Units.Value;
        }
    }
}
