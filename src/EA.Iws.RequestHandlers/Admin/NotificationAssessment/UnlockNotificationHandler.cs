namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class UnlockNotificationHandler : IRequestHandler<UnlockNotification, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository assessmentRepository;

        public UnlockNotificationHandler(INotificationAssessmentRepository assessmentRepository,
            IwsContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(UnlockNotification message)
        {
            var assessment = await assessmentRepository.GetByNotificationId(message.NotificationId);

            assessment.Unlock();

            await context.SaveChangesAsync();

            return true;
        }
    }
}