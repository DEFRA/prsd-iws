namespace EA.Iws.RequestHandlers.Movement.Complete
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement.Complete;

    public class RecordOperationCompleteInternalHandler : IRequestHandler<RecordOperationCompleteInternal, bool>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public RecordOperationCompleteInternalHandler(IMovementRepository movementRepository, IwsContext context, IUserContext userContext)
        {
            this.movementRepository = movementRepository;
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(RecordOperationCompleteInternal message)
        {
            var movement = await movementRepository.GetById(message.Id);

            movement.CompleteInternally(message.CompleteDate, userContext.UserId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
