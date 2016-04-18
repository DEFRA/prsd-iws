namespace EA.Iws.RequestHandlers.Notification
{
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Domain;
    using System.Threading.Tasks;

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
            var financialGuarantee = FinancialGuarantee.Create(@event.Notification.Id);

            context.NotificationAssessments.Add(notificationAssessment);
            context.FinancialGuarantees.Add(financialGuarantee);

            await context.SaveChangesAsync();
        }
    }
}