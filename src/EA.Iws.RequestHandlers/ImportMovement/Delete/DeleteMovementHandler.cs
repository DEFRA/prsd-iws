namespace EA.Iws.RequestHandlers.ImportMovement.Delete
{
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Delete;

    internal class DeleteMovementHandler : IRequestHandler<DeleteMovement, bool>
    {
        private readonly IImportMovementRepository repository;

        public DeleteMovementHandler(IImportMovementRepository repository)
        {
            this.repository = repository;
        }

        public Task<bool> HandleAsync(DeleteMovement message)
        {
            return repository.DeleteById(message.MovementId);
        }
    }
}
