namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;
    using PackagingTypeEnum = Core.PackagingType.PackagingType;

    internal class CreateMovementAndDetailsHandler : IRequestHandler<CreateMovementAndDetails, Guid>
    {
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IwsContext context;
        private readonly MovementFactory movementFactory;
        private readonly MovementDetailsFactory movementDetailsFactory;

        public CreateMovementAndDetailsHandler(MovementFactory movementFactory,
            MovementDetailsFactory movementDetailsFactory,
            INotificationApplicationRepository notificationRepository,
            IwsContext context)
        {
            this.movementFactory = movementFactory;
            this.movementDetailsFactory = movementDetailsFactory;
            this.context = context;
            this.notificationRepository = notificationRepository;
        }

        public async Task<Guid> HandleAsync(CreateMovementAndDetails message)
        {
            var movement = await movementFactory.Create(message.NotificationId, message.ActualMovementDate);

            context.Movements.Add(movement);

            await context.SaveChangesAsync();

            var newMovementDetails = message.NewMovementDetails;
            var shipmentQuantity = new ShipmentQuantity(newMovementDetails.Quantity, newMovementDetails.Units);
            var carriers = await GetCarriers(message.NotificationId, newMovementDetails.OrderedCarriers);
            var packagingInfos = await GetPackagingInfos(message.NotificationId, newMovementDetails.PackagingTypes);

            var movementDetails = await movementDetailsFactory.Create(
                movement,
                shipmentQuantity,
                newMovementDetails.NumberOfPackages,
                carriers,
                packagingInfos);

            context.MovementDetails.Add(movementDetails);

            await context.SaveChangesAsync();

            return movement.Id;
        }

        private async Task<IEnumerable<PackagingInfo>> GetPackagingInfos(Guid notificationId, IList<PackagingTypeEnum> packagingTypes)
        {
            var notification = await notificationRepository.GetById(notificationId);

            return notification.PackagingInfos
                .Where(p => 
                    packagingTypes
                        .Select(x => (int)x)
                        .Contains(p.PackagingType.Value));
        }

        private async Task<IEnumerable<MovementCarrier>> GetCarriers(Guid notificationId, Dictionary<int, Guid> orderedCarriers)
        {
            var notification = await notificationRepository.GetById(notificationId);

            return notification.Carriers.Join(orderedCarriers,
                notificationCarrier => notificationCarrier.Id,
                orderedCarrier => orderedCarrier.Value,
                (notificationCarrier, orderedCarrier) => 
                    new MovementCarrier(orderedCarrier.Key, notificationCarrier));
        }
    }
}