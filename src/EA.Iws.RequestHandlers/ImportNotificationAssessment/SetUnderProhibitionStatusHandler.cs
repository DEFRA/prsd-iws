namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    public class SetUnderProhibitionStatusHandler : IRequestHandler<SetUnderProhibitionStatus, bool>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly ImportNotificationContext context;

        public SetUnderProhibitionStatusHandler(IImportNotificationAssessmentRepository assessmentRepository,
            ImportNotificationContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetUnderProhibitionStatus message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.ImportNotificationId);
            // get previous status
            assessment.UnderProhibition(DateTime.Now);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
