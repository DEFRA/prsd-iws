namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class SubmitNotificationHandler : IRequestHandler<SubmitNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationProgressService progressService;

        public SubmitNotificationHandler(IwsContext context, INotificationProgressService progressService)
        {
            this.context = context;
            this.progressService = progressService;
        }

        public async Task<Guid> HandleAsync(SubmitNotification message)
        {
            var assessment =
                await
                    context.NotificationAssessments.SingleAsync(
                        p => p.NotificationApplicationId == message.NotificationId);

            assessment.Submit(progressService);

            await context.SaveChangesAsync();

            return assessment.Id;
        }
    }
}