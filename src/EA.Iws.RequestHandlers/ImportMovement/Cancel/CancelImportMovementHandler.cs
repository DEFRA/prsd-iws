namespace EA.Iws.RequestHandlers.ImportMovement.Cancel
{
    using System.Threading.Tasks;
    using Prsd.Core.Mediator;
    using CancelImportMovement = Requests.ImportMovement.Cancel.CancelImportMovement;

    internal class CancelImportMovementHandler : IRequestHandler<CancelImportMovement, bool>
    {
        private readonly Domain.ImportMovement.CancelImportMovement cancelMovement;

        public CancelImportMovementHandler(Domain.ImportMovement.CancelImportMovement cancelMovement)
        {
            this.cancelMovement = cancelMovement;
        }

        public async Task<bool> HandleAsync(CancelImportMovement message)
        {
            await cancelMovement.Cancel(message.MovementId);
            return true;
        }
    }
}