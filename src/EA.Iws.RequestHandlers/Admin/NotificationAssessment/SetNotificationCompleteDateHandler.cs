namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetNotificationCompleteDateHandler : IRequestHandler<SetNotificationCompleteDate, bool>
    {
        private readonly IwsContext context;
        private readonly CompleteNotification completeNotification;

        public SetNotificationCompleteDateHandler(IwsContext context, CompleteNotification completeNotification)
        {
            this.context = context;
            this.completeNotification = completeNotification;
        }

        public async Task<bool> HandleAsync(SetNotificationCompleteDate message)
        {
            await completeNotification.Complete(message.NotificationId, message.NotificationCompleteDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}