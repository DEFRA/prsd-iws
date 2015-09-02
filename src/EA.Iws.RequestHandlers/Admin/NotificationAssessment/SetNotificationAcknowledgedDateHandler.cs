namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetNotificationAcknowledgedDateHandler : IRequestHandler<SetNotificationAcknowledgedDate, bool>
    {
        private readonly IwsContext context;

        public SetNotificationAcknowledgedDateHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetNotificationAcknowledgedDate message)
        {
            var assessment =
                await
                    context.NotificationAssessments.SingleAsync(
                        p => p.NotificationApplicationId == message.NotificationId);

            assessment.Acknowledge(message.AcknowledgedDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}