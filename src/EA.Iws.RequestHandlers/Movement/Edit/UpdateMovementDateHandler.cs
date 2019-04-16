namespace EA.Iws.RequestHandlers.Movement.Edit
{
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement.Edit;

    internal class UpdateMovementDateHandler : IRequestHandler<UpdateMovementDate, bool>
    {
        private readonly IUpdatedMovementDateValidator validator;
        private readonly IwsContext context;
        private readonly IMovementRepository repository;
        private readonly IMovementAuditRepository movementAuditRepository;
        private readonly IUserContext userContext;

        public UpdateMovementDateHandler(IMovementRepository repository,
            IUpdatedMovementDateValidator validator,
            IwsContext context,
            IMovementAuditRepository movementAuditRepository,
            IUserContext userContext)
        {
            this.repository = repository;
            this.context = context;
            this.validator = validator;
            this.movementAuditRepository = movementAuditRepository;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(UpdateMovementDate message)
        {
            var movement = await repository.GetById(message.MovementId);

            await movement.UpdateDate(message.NewDate, validator);

            await context.SaveChangesAsync();

            await movementAuditRepository.Add(new MovementAudit(movement.NotificationId, movement.Number,
                userContext.UserId.ToString(), (int)MovementAuditType.Edited, SystemTime.UtcNow));

            await context.SaveChangesAsync();

            return true;
        }
    }
}