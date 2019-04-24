namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.PackagingType;
    using DataAccess;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class CreateMovementsHandler : IRequestHandler<CreateMovements, Guid[]>
    {
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IwsContext context;
        private readonly MovementFactory movementFactory;
        private readonly MovementDetailsFactory movementDetailsFactory;
        private readonly IMovementAuditRepository movementAuditRepository;
        private readonly IUserContext userContext;

        public CreateMovementsHandler(MovementFactory movementFactory,
            MovementDetailsFactory movementDetailsFactory,
            INotificationApplicationRepository notificationRepository,
            IwsContext context,
            IMovementAuditRepository movementAuditRepository,
            IUserContext userContext)
        {
            this.movementFactory = movementFactory;
            this.movementDetailsFactory = movementDetailsFactory;
            this.context = context;
            this.notificationRepository = notificationRepository;
            this.movementAuditRepository = movementAuditRepository;
            this.userContext = userContext;
        }

        public async Task<Guid[]> HandleAsync(CreateMovements message)
        {
            var newIds = new List<Guid>();

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

                        newIds.Add(movement.Id);

                        await movementAuditRepository.Add(new MovementAudit(movement.NotificationId, movement.Number,
                            userContext.UserId.ToString(), (int)MovementAuditType.Incomplete, SystemTime.Now));

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

            return newIds.ToArray();
        }

        private async Task<IEnumerable<PackagingInfo>> GetPackagingInfos(Guid notificationId, IList<PackagingType> packagingTypes)
        {
            var notification = await notificationRepository.GetById(notificationId);

            return notification.PackagingInfos
                .Where(p => packagingTypes.Contains(p.PackagingType));
        }
    }
}