namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Domain;

    public class NotificationStatusChangeEventHandler : IEventHandler<NotificationStatusChangeEvent>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public NotificationStatusChangeEventHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task HandleAsync(NotificationStatusChangeEvent @event)
        {
            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());

            @event.NotificationAssessment.AddStatusChangeRecord(new NotificationStatusChange(@event.TargetStatus, user));

            await context.SaveChangesAsync();
        }
    }
}