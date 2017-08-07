namespace EA.Iws.RequestHandlers.ImportMovement
{
    using Domain.ImportMovement;
    using EA.Iws.Requests.ImportMovement;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using System.Threading.Tasks;

    public class SetImportMovementReceiptAndRecoveryDataHandler : IRequestHandler<SetImportMovementReceiptAndRecoveryData, Unit>
    {
        private readonly IUserContext userContext;
        private readonly IImportMovementRepository movementRepository;
        public SetImportMovementReceiptAndRecoveryDataHandler(IImportMovementRepository movementRepository, IUserContext userContext)
        {
            this.movementRepository = movementRepository;
            this.userContext = userContext;
        }

        public async Task<Unit> HandleAsync(SetImportMovementReceiptAndRecoveryData message)
        {
            await movementRepository.SetImportMovementReceiptAndRecoveryData(message.Data, userContext.UserId);
            return Unit.Value;
        }
    }
}
