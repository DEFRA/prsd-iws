namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    public class RemoveUnderProhibitionStatusHandler : IRequestHandler<RemoveUnderProhibitionStatus, bool>
    {
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IwsContext context;

        public RemoveUnderProhibitionStatusHandler(INotificationAssessmentRepository assessmentRepository,
           IwsContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RemoveUnderProhibitionStatus message)
        {
            var assessment = await assessmentRepository.GetByNotificationId(message.NotificationId);

            var previousStatus = await assessmentRepository.GetPreviousStatusByNotification(message.NotificationId);

            assessment.LiftProhibition(DateTime.UtcNow, previousStatus);

            await context.SaveChangesAsync();

            return true;
        }
    }
}