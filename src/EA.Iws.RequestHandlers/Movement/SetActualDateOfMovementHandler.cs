namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SetActualDateOfMovementHandler : IRequestHandler<SetActualDateOfMovement, Guid>
    {
        private readonly IwsContext context;
        private readonly SetActualDateOfShipment setActualDateOfShipment;

        public SetActualDateOfMovementHandler(IwsContext context, SetActualDateOfShipment setActualDateOfShipment)
        {
            this.context = context;
            this.setActualDateOfShipment = setActualDateOfShipment;
        }

        public async Task<Guid> HandleAsync(SetActualDateOfMovement command)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == command.MovementId);
            var notification = await context.GetNotificationApplication(movement.NotificationId);

            setActualDateOfShipment.Apply(command.Date, movement, notification.ShipmentInfo);

            await context.SaveChangesAsync();

            return command.MovementId;
        }
    }
}
