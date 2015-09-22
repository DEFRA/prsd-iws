namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SetActualMovementCarriersHandler : IRequestHandler<SetActualMovementCarriers, bool>
    {
        private readonly IwsContext context;

        public SetActualMovementCarriersHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetActualMovementCarriers message)
        {
            var movement = await context.Movements.Where(m => m.Id == message.MovementId).SingleAsync();
            var notification = await context.GetNotificationApplication(movement.NotificationId);

            var movementCarriersData = notification.Carriers
                .Join(message.SelectedCarriers,
                    c => c.Id,
                    sc => sc.Value,
                    (carrier, order) => new { Order = order.Key, Carrier = carrier });

            var movementCarriers = new List<MovementCarrier>();

            foreach (var item in movementCarriersData)
            {
                movementCarriers.Add(new MovementCarrier(item.Order, item.Carrier));
            }

            movement.SetMovementCarriers(movementCarriers);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
