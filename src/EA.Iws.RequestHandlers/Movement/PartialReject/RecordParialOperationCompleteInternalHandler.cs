namespace EA.Iws.RequestHandlers.Movement.PartialReject
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using EA.Iws.Core.Movement;
    using EA.Iws.Requests.Movement.PartialReject;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement.Complete;

    public class RecordParialOperationCompleteInternalHandler : IRequestHandler<RecordParialOperationCompleteInternal, bool>
    {
        private readonly IMovementPartialRejectionRepository movementPartialRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IMovementRepository movementRepository;

        public RecordParialOperationCompleteInternalHandler(IMovementPartialRejectionRepository movementPartialRepository, IwsContext context, IUserContext userContext,
            IMovementRepository movementRepository)
        {
            this.movementPartialRepository = movementPartialRepository;
            this.context = context;
            this.userContext = userContext;
            this.movementRepository = movementRepository;
        }

        public async Task<bool> HandleAsync(RecordParialOperationCompleteInternal message)
        {
            var movementPartial = await movementPartialRepository.GetByMovementId(message.Id);
            movementPartial.WasteDisposedDate = message.CompleteDate;

            var movement = await movementRepository.GetById(message.Id);
            if (movement.CompletedReceipt == null)
            {
                movement.Complete(message.CompleteDate.Value, System.Guid.NewGuid(), userContext.UserId);
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
