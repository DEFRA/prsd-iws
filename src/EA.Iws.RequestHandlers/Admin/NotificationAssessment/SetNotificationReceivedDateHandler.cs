namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetNotificationReceivedDateHandler : IRequestHandler<SetNotificationReceivedDate, bool>
    {
        private readonly IwsContext context;

        public SetNotificationReceivedDateHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetNotificationReceivedDate message)
        {
            var assessment =
                await
                    context.NotificationAssessments.SingleAsync(
                        p => p.NotificationApplicationId == message.NotificationId);

            assessment.NotificationReceived(message.NotificationReceivedDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}