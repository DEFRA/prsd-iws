namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SetMovementCommentsHandler : IRequestHandler<SetMovementComments, Unit>
    {
        private readonly IwsContext context;
        private readonly IMovementRepository repository;

        public SetMovementCommentsHandler(IMovementRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetMovementComments message)
        {
            var movement = await repository.GetById(message.MovementId);
            if (!string.IsNullOrWhiteSpace(message.Comments))
            {
                movement.SetComments(message.Comments);
            }
            if (!string.IsNullOrWhiteSpace(message.StatsMarking))
            {
                movement.SetStatsMarking(message.StatsMarking);
            }

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}