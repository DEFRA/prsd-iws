namespace EA.Iws.RequestHandlers.Movement.Complete
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Complete;

    public class RecordOperationCompleteInternalHandler : IRequestHandler<RecordOperationCompleteInternal, bool>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;

        public RecordOperationCompleteInternalHandler(IMovementRepository movementRepository, IwsContext context)
        {
            this.movementRepository = movementRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordOperationCompleteInternal message)
        {
            var movement = await movementRepository.GetById(message.Id);

            movement.CompleteInternally(message.CompleteDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
