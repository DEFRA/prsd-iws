namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    public class SetReceivedDateHandler : IRequestHandler<SetReceivedDate, bool>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly ImportNotificationContext context;

        public SetReceivedDateHandler(IImportNotificationAssessmentRepository assessmentRepository,
            ImportNotificationContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetReceivedDate message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.ImportNotificationId);

            assessment.Receive(message.ReceivedDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
