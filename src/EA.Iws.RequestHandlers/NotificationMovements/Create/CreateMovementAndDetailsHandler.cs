namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.PackagingType;
    using DataAccess;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class CreateMovementAndDetailsHandler : IRequestHandler<CreateMovementAndDetails, Guid>
    {
        private readonly ICarrierRepository carrierRepository;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IwsContext context;
        private readonly MovementFactory movementFactory;
        private readonly MovementDetailsFactory movementDetailsFactory;

        public CreateMovementAndDetailsHandler(MovementFactory movementFactory,
            MovementDetailsFactory movementDetailsFactory,
            INotificationApplicationRepository notificationRepository,
            ICarrierRepository carrierRepository,
            IwsContext context)
        {
            this.movementFactory = movementFactory;
            this.movementDetailsFactory = movementDetailsFactory;
            this.context = context;
            this.notificationRepository = notificationRepository;
            this.carrierRepository = carrierRepository;
        }

        public async Task<Guid> HandleAsync(CreateMovementAndDetails message)
        {
            var movement = await movementFactory.Create(message.NotificationId, message.ActualMovementDate);

            context.Movements.Add(movement);

            await context.SaveChangesAsync();

            var newMovementDetails = message.NewMovementDetails;
            var shipmentQuantity = new ShipmentQuantity(newMovementDetails.Quantity, newMovementDetails.Units);
            var packagingInfos = await GetPackagingInfos(message.NotificationId, newMovementDetails.PackagingTypes);

            var movementDetails = await movementDetailsFactory.Create(
                movement,
                shipmentQuantity,
                packagingInfos);

            context.MovementDetails.Add(movementDetails);

            await context.SaveChangesAsync();

            return movement.Id;
        }

        private async Task<IEnumerable<PackagingInfo>> GetPackagingInfos(Guid notificationId, IList<PackagingType> packagingTypes)
        {
            var notification = await notificationRepository.GetById(notificationId);

            return notification.PackagingInfos
                .Where(p => packagingTypes.Contains(p.PackagingType));
        }

        private async Task<IEnumerable<MovementCarrier>> GetCarriers(Guid notificationId, Dictionary<int, Guid> orderedCarriers)
        {
            var carrierCollection = await carrierRepository.GetByNotificationId(notificationId);

            return carrierCollection.Carriers.Join(orderedCarriers,
                notificationCarrier => notificationCarrier.Id,
                orderedCarrier => orderedCarrier.Value,
                (notificationCarrier, orderedCarrier) =>
                    new MovementCarrier(orderedCarrier.Key, notificationCarrier));
        }
    }
}