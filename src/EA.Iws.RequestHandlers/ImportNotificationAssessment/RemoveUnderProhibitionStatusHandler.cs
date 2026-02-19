namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    public class RemoveUnderProhibitionStatusHandler : IRequestHandler<RemoveUnderProhibitionStatus, bool>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly ImportNotificationContext context;

        public RemoveUnderProhibitionStatusHandler(IImportNotificationAssessmentRepository assessmentRepository,
            ImportNotificationContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RemoveUnderProhibitionStatus message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.ImportNotificationId);

            var previousStatusChange = await assessmentRepository.GetPreviousStatusChangeByNotification(message.ImportNotificationId);
            var previousStatus = previousStatusChange.PreviousStatus;

            assessment.LiftProhibition(DateTime.Now, previousStatus);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
