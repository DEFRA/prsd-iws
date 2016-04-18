namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class SubmitNotificationHandler : IRequestHandler<SubmitNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationProgressService progressService;
        private readonly INotificationAssessmentRepository repository;

        public SubmitNotificationHandler(IwsContext context, INotificationProgressService progressService, INotificationAssessmentRepository repository)
        {
            this.context = context;
            this.progressService = progressService;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(SubmitNotification message)
        {
            var assessment = await repository.GetByNotificationId(message.NotificationId);

            assessment.Submit(progressService);

            await context.SaveChangesAsync();

            return assessment.Id;
        }
    }
}