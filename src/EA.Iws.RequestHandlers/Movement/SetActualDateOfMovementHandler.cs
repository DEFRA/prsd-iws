namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;

    internal class SetActualDateOfMovementHandler : IRequestHandler<SetActualDateOfMovement, Guid>
    {
        private readonly IwsContext context;
        private readonly SetActualDateOfShipment setActualDateOfShipment;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public SetActualDateOfMovementHandler(IwsContext context, 
            SetActualDateOfShipment setActualDateOfShipment,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.context = context;
            this.setActualDateOfShipment = setActualDateOfShipment;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<Guid> HandleAsync(SetActualDateOfMovement command)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == command.MovementId);
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(movement.NotificationId);

            setActualDateOfShipment.Apply(command.Date, movement, shipmentInfo);

            await context.SaveChangesAsync();

            return command.MovementId;
        }
    }
}
