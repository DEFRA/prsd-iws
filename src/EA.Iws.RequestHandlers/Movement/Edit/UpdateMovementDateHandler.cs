namespace EA.Iws.RequestHandlers.Movement.Edit
{
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Edit;

    internal class UpdateMovementDateHandler : IRequestHandler<UpdateMovementDate, bool>
    {
        private readonly IUpdatedMovementDateValidator validator;
        private readonly IwsContext context;
        private readonly IMovementRepository repository;
        private readonly IMovementAuditRepository movementAuditRepository;

        public UpdateMovementDateHandler(IMovementRepository repository,
            IUpdatedMovementDateValidator validator,
            IwsContext context,
            IMovementAuditRepository movementAuditRepository)
        {
            this.repository = repository;
            this.context = context;
            this.validator = validator;
            this.movementAuditRepository = movementAuditRepository;
        }

        public async Task<bool> HandleAsync(UpdateMovementDate message)
        {
            var movement = await repository.GetById(message.MovementId);

            await movement.UpdateDate(message.NewDate, validator);

            await movementAuditRepository.Add(movement, MovementAuditType.Edited);

            await context.SaveChangesAsync();

            return true;
        }
    }
}