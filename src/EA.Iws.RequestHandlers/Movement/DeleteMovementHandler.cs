namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class DeleteMovementHandler : IRequestHandler<DeleteMovement, bool>
    {
        private readonly IMovementRepository repository;

        public DeleteMovementHandler(IMovementRepository repository)
        {
            this.repository = repository;
        }

        public Task<bool> HandleAsync(DeleteMovement message)
        {
            return repository.DeleteById(message.MovementId);
        }
    }
}
