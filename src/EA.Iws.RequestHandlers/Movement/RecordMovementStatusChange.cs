namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Domain;

    public class RecordMovementStatusChange : IEventHandler<MovementStatusChangeEvent>
    {
        private readonly IUserContext userContext;
        private readonly IwsContext context;

        public RecordMovementStatusChange(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task HandleAsync(MovementStatusChangeEvent @event)
        {
            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());

            @event.Movement.AddStatusChangeRecord(new MovementStatusChange(@event.TargetStatus, user));

            await context.SaveChangesAsync();
        }
    }
}
