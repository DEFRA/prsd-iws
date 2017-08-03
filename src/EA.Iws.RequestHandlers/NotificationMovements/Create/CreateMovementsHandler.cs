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

    internal class CreateMovementsHandler : IRequestHandler<CreateMovements, Unit>
    {
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IwsContext context;
        private readonly MovementFactory movementFactory;
        private readonly MovementDetailsFactory movementDetailsFactory;

        public CreateMovementsHandler(MovementFactory movementFactory,
            MovementDetailsFactory movementDetailsFactory,
            INotificationApplicationRepository notificationRepository,
            IwsContext context)
        {
            this.movementFactory = movementFactory;
            this.movementDetailsFactory = movementDetailsFactory;
            this.context = context;
            this.notificationRepository = notificationRepository;
        }

        public async Task<Unit> HandleAsync(CreateMovements message)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < message.NumberToCreate; i++)
                    {
                        var movement = await movementFactory.Create(message.NotificationId, message.ActualMovementDate);

                        context.Movements.Add(movement);

                        await context.SaveChangesAsync();

                        var shipmentQuantity = new ShipmentQuantity(message.Quantity, message.Units);
                        var packagingInfos = await GetPackagingInfos(message.NotificationId, message.PackagingTypes);

                        var movementDetails = await movementDetailsFactory.Create(
                            movement,
                            shipmentQuantity,
                            packagingInfos);

                        context.MovementDetails.Add(movementDetails);

                        await context.SaveChangesAsync();
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

                transaction.Commit();
            }

            return Unit.Value;
        }

        private async Task<IEnumerable<PackagingInfo>> GetPackagingInfos(Guid notificationId, IList<PackagingType> packagingTypes)
        {
            var notification = await notificationRepository.GetById(notificationId);

            return notification.PackagingInfos
                .Where(p => packagingTypes.Contains(p.PackagingType));
        }
    }
}