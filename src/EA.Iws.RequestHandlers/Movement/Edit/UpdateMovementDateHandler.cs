namespace EA.Iws.RequestHandlers.Movement.Edit
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Edit;

    internal class UpdateMovementDateHandler : IRequestHandler<UpdateMovementDate, bool>
    {
        private readonly IUpdatedMovementDateValidator validator;
        private readonly IwsContext context;
        private readonly IMovementRepository repository;

        public UpdateMovementDateHandler(IMovementRepository repository,
            IUpdatedMovementDateValidator validator,
            IwsContext context)
        {
            this.repository = repository;
            this.context = context;
            this.validator = validator;
        }

        public async Task<bool> HandleAsync(UpdateMovementDate message)
        {
            var movement = await repository.GetById(message.MovementId);

            await movement.UpdateDate(message.NewDate, validator);

            await context.SaveChangesAsync();

            return true;
        }
    }
}