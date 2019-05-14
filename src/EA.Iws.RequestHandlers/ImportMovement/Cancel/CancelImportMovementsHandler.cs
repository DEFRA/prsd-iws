namespace EA.Iws.RequestHandlers.ImportMovement.Cancel
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Cancel;

    internal class CancelImportMovementsHandler : IRequestHandler<CancelImportMovements, bool>
    {
        private readonly Domain.ImportMovement.CancelImportMovement cancelMovement;
        private readonly ImportNotificationContext context;
        private readonly IImportMovementAuditRepository repository;
        private readonly IUserContext userContext;
        private readonly IImportMovementAuditRepository movementAuditRepository;
        private readonly IImportMovementFactory importMovementFactory;
        private readonly IImportMovementRepository movementRepository;
        // Add delay to the audit time to ensure this is logged after Prenotified audit.
        private const int AuditTimeOffSet = 2;
        public CancelImportMovementsHandler(Domain.ImportMovement.CancelImportMovement cancelMovement, ImportNotificationContext context, IImportMovementAuditRepository repository,
            IUserContext userContext, IImportMovementAuditRepository movementAuditRepository, IImportMovementFactory importMovementFactory, IImportMovementRepository movementRepository)
        {
            this.cancelMovement = cancelMovement;
            this.context = context;
            this.repository = repository;
            this.userContext = userContext;
            this.movementAuditRepository = movementAuditRepository;
            this.importMovementFactory = importMovementFactory;
            this.movementRepository = movementRepository;
        }

        public async Task<bool> HandleAsync(CancelImportMovements message)
        {
            var movementIds = message.CancelledMovements.Select(m => m.Id).ToList();

            movementIds.AddRange((await CaptureAddedImportMovements(message)).Select(m => m.Id));

            var movements = (await movementRepository.GetImportMovementsByIds(message.NotificationId, movementIds)).ToList();

            foreach (var movement in movements)
            {
                await cancelMovement.Cancel(movement.Id);
            }

            await context.SaveChangesAsync();

            foreach (var movement in movements)
            {
                await
                    repository.Add(new ImportMovementAudit(message.NotificationId, movement.Number,
                        userContext.UserId.ToString().ToUpper(), (int)MovementAuditType.Cancelled, SystemTime.Now.AddSeconds(AuditTimeOffSet)));
            }

            await context.SaveChangesAsync();

            return true;
        }

        private async Task<List<ImportMovement>> CaptureAddedImportMovements(CancelImportMovements message)
        {
            var result = new List<ImportMovement>();

            foreach (var addedMovement in message.AddedMovements)
            {
                var movement = await importMovementFactory.Create(message.NotificationId, addedMovement.Number,
                     addedMovement.ShipmentDate, null);

                movement.SetPrenotificationDate(SystemTime.Now.Date);

                movementRepository.Add(movement);

                await context.SaveChangesAsync();

                result.Add(movement);
            }

            foreach (var movement in result)
            {
                await movementAuditRepository.Add(new ImportMovementAudit(movement.NotificationId, movement.Number,
                    userContext.UserId.ToString(), (int)MovementAuditType.Prenotified, SystemTime.Now));
            }

            await context.SaveChangesAsync();

            return result;
        }
    }
}