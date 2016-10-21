namespace EA.Iws.RequestHandlers.ImportMovement.Cancel
{
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Cancel;

    internal class CancelImportMovementsHandler : IRequestHandler<CancelImportMovements, bool>
    {
        private readonly Domain.ImportMovement.CancelImportMovement cancelMovement;
        private readonly ImportNotificationContext context;

        public CancelImportMovementsHandler(Domain.ImportMovement.CancelImportMovement cancelMovement, ImportNotificationContext context)
        {
            this.cancelMovement = cancelMovement;
            this.context = context;
        }

        public async Task<bool> HandleAsync(CancelImportMovements message)
        {
            var movementIds = message.CancelledMovements.Select(m => m.Id);

            foreach (var movementId in movementIds)
            {
                await cancelMovement.Cancel(movementId);
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}