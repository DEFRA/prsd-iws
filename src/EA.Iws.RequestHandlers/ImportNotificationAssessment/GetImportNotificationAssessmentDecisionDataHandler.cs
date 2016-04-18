namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportNotificationAssessmentDecisionDataHandler : IRequestHandler<GetImportNotificationAssessmentDecisionData, ImportNotificationAssessmentDecisionData>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;

        public GetImportNotificationAssessmentDecisionDataHandler(IImportNotificationAssessmentRepository assessmentRepository)
        {
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<ImportNotificationAssessmentDecisionData> HandleAsync(GetImportNotificationAssessmentDecisionData message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.ImportNotificationId);

            return new ImportNotificationAssessmentDecisionData
            {
                Status = assessment.Status,
                AvailableDecisions = assessment.GetAvailableDecisions().ToArray()
            };
        }
    }
}
