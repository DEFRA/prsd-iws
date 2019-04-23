namespace EA.Iws.RequestHandlers.ImportMovement.Cancel
{
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

        public CancelImportMovementsHandler(Domain.ImportMovement.CancelImportMovement cancelMovement, ImportNotificationContext context, IImportMovementAuditRepository repository, IUserContext userContext)
        {
            this.cancelMovement = cancelMovement;
            this.context = context;
            this.repository = repository;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(CancelImportMovements message)
        {
            var movementIds = message.CancelledMovements.Select(m => m.Id);

            foreach (var movementId in movementIds)
            {
                await cancelMovement.Cancel(movementId);
            }

            await context.SaveChangesAsync();

            foreach (var movement in message.CancelledMovements)
            {
                await
                    repository.Add(new ImportMovementAudit(message.NotificationId, movement.Number,
                        userContext.UserId.ToString().ToUpper(), (int)MovementAuditType.Cancelled, SystemTime.Now));
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}