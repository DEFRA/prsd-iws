namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetNotificationCompleteDateHandler : IRequestHandler<SetNotificationCompleteDate, bool>
    {
        private readonly IwsContext context;

        public SetNotificationCompleteDateHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetNotificationCompleteDate message)
        {
            var assessment =
                await
                    context.NotificationAssessments.SingleAsync(
                        p => p.NotificationApplicationId == message.NotificationId);

            assessment.SetNotificationCompleted(message.NotificationCompleteDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}