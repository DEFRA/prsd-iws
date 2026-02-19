namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    public class SetImportNotificationUnderProhibitionStatusHandler : IRequestHandler<SetImportNotificationUnderProhibitionStatus, bool>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly ImportNotificationContext context;

        public SetImportNotificationUnderProhibitionStatusHandler(IImportNotificationAssessmentRepository assessmentRepository,
            ImportNotificationContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetImportNotificationUnderProhibitionStatus message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.ImportNotificationId);
            // get previous status
            assessment.UnderProhibition(DateTime.Now);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
