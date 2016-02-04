namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetKeyDatesSummaryInformationHandler : IRequestHandler<GetKeyDatesSummaryInformation, KeyDatesSummaryData>
    {
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IFinancialGuaranteeDecisionRepository financialGuaranteeDecisionRepository;
        private readonly INotificationAssessmentDatesSummaryRepository datesSummaryRepository;
        private readonly INotificationAssessmentDecisionRepository decisionRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly IMapper mapper;

        public GetKeyDatesSummaryInformationHandler(INotificationApplicationRepository notificationRepository, 
            INotificationAssessmentRepository assessmentRepository,
            IFinancialGuaranteeDecisionRepository financialGuaranteeDecisionRepository,
            INotificationAssessmentDatesSummaryRepository datesSummaryRepository,
            INotificationAssessmentDecisionRepository decisionRepository,
            IFacilityRepository facilityRepository,
            IMapper mapper)
        {
            this.notificationRepository = notificationRepository;
            this.assessmentRepository = assessmentRepository;
            this.financialGuaranteeDecisionRepository = financialGuaranteeDecisionRepository;
            this.datesSummaryRepository = datesSummaryRepository;
            this.decisionRepository = decisionRepository;
            this.facilityRepository = facilityRepository;
            this.mapper = mapper;
        }

        public async Task<KeyDatesSummaryData> HandleAsync(GetKeyDatesSummaryInformation message)
        {
            var notification = await notificationRepository.GetById(message.NotificationId);

            var assessment = await assessmentRepository.GetByNotificationId(message.NotificationId);

            var dates = await datesSummaryRepository.GetById(message.NotificationId);

            var financialGuaranteeDecisions = await financialGuaranteeDecisionRepository.GetByNotificationId(message.NotificationId);

            var decision = await decisionRepository.GetByNotificationId(message.NotificationId);

            var facilityCollection = await facilityRepository.GetByNotificationId(message.NotificationId);

            return new KeyDatesSummaryData
            {
                CompetentAuthority = notification.CompetentAuthority,
                IsLocalAreaSet = assessment.LocalAreaId.HasValue,
                FinancialGuaranteeDecisions =
                    financialGuaranteeDecisions.Select(x => mapper.Map<FinancialGuaranteeDecisionData>(x)).ToArray(),
                Dates = mapper.Map<NotificationDatesData>(dates),
                DecisionHistory = decision,
                IsInterim = facilityCollection.IsInterim
            };
        }
    }
}
