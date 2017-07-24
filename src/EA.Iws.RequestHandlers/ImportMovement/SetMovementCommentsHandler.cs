namespace EA.Iws.RequestHandlers.ImportMovement
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;

    internal class SetMovementCommentsHandler : IRequestHandler<SetMovementComments, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly IImportMovementRepository repository;

        public SetMovementCommentsHandler(IImportMovementRepository repository, ImportNotificationContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetMovementComments message)
        {
            var movement = await repository.Get(message.MovementId);
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