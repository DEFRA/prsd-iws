namespace EA.Iws.RequestHandlers.Movement.Edit
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Edit;

    internal class UpdateMovementDateHandler : IRequestHandler<UpdateMovementDate, bool>
    {
        private readonly IwsContext context;
        private readonly GetOriginalDate originalDateService;
        private readonly IMovementRepository repository;

        public UpdateMovementDateHandler(IMovementRepository repository,
            GetOriginalDate originalDateService,
            IwsContext context)
        {
            this.repository = repository;
            this.originalDateService = originalDateService;
            this.context = context;
        }

        public async Task<bool> HandleAsync(UpdateMovementDate message)
        {
            var movement = await repository.GetById(message.MovementId);

            await movement.UpdateDate(message.NewDate, originalDateService);

            await context.SaveChangesAsync();

            return true;
        }
    }
}