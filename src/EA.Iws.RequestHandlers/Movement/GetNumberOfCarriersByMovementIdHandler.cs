namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetNumberOfCarriersByMovementIdHandler : IRequestHandler<GetNumberOfCarriersByMovementId, int?>
    {
        private readonly IwsContext context;

        public GetNumberOfCarriersByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<int?> HandleAsync(GetNumberOfCarriersByMovementId message)
        {
            var movement = await context.Movements.Where(m => m.Id == message.MovementId).SingleAsync();

            if (movement.MovementCarriers.Count() == 0)
            {
                return null;
            }

            return movement.MovementCarriers.Count();
        }
    }
}
