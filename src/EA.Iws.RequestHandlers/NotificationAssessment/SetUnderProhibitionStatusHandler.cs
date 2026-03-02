namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    public class SetUnderProhibitionStatusHandler : IRequestHandler<SetUnderProhibitionStatus, bool>
    {
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IwsContext context;

        public SetUnderProhibitionStatusHandler(INotificationAssessmentRepository assessmentRepository,
            IwsContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetUnderProhibitionStatus message)
        {
            var assessment = await assessmentRepository.GetByNotificationId(message.NotificationId);

            if (assessment == null)
            {
                return false;
            }

            assessment.UnderProhibition(DateTime.UtcNow);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
