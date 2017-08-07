namespace EA.Iws.RequestHandlers.Movement
{
    using Domain.Movement;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using System.Threading.Tasks;

    public class SetMovementReceiptAndRecoveryDataHandler : IRequestHandler<SetMovementReceiptAndRecoveryData, Unit>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IUserContext userContext;
        public SetMovementReceiptAndRecoveryDataHandler(IMovementRepository movementRepository, IUserContext userContext)
        {
            this.movementRepository = movementRepository;
            this.userContext = userContext;
        }

        public async Task<Unit> HandleAsync(SetMovementReceiptAndRecoveryData message)
        {
            await movementRepository.SetMovementReceiptAndRecoveryData(message.Data, userContext.UserId);
            return Unit.Value;
        }
    }
}
