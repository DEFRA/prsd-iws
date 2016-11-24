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
        private readonly IFacilityRepository facilityRepository;

        public GetImportNotificationAssessmentDecisionDataHandler(IImportNotificationAssessmentRepository assessmentRepository, IFacilityRepository facilityRepository)
        {
            this.assessmentRepository = assessmentRepository;
            this.facilityRepository = facilityRepository;
        }

        public async Task<ImportNotificationAssessmentDecisionData> HandleAsync(GetImportNotificationAssessmentDecisionData message)
        {
            var isPreconsented = (await facilityRepository.GetByNotificationId(message.ImportNotificationId)).AllFacilitiesPreconsented;
            var assessment = await assessmentRepository.GetByNotification(message.ImportNotificationId);

            return new ImportNotificationAssessmentDecisionData
            {
                Status = assessment.Status,
                AvailableDecisions = assessment.GetAvailableDecisions().ToArray(),
                AcknowledgedOnDate = assessment.Dates.AcknowledgedDate.GetValueOrDefault(),
                IsPreconsented = isPreconsented,
                NotificationReceivedDate = assessment.Dates.NotificationReceivedDate.GetValueOrDefault()
            };
        }
    }
}
