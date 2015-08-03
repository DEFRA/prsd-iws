namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Domain;

    internal class NotificationCreatedEventHandler : IEventHandler<NotificationCreatedEvent>
    {
        private readonly IwsContext context;

        public NotificationCreatedEventHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task HandleAsync(NotificationCreatedEvent @event)
        {
            var notificationAssessment = new NotificationAssessment(@event.Notification.Id);

            context.NotificationAssessments.Add(notificationAssessment);

            await context.SaveChangesAsync();
        }
    }
}