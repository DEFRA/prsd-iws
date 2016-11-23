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
        private readonly IConsultationRepository consultationRepository;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly INotificationAssessmentDatesSummaryRepository datesSummaryRepository;
        private readonly INotificationAssessmentDecisionRepository decisionRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly IMapper mapper;

        public GetKeyDatesSummaryInformationHandler(INotificationApplicationRepository notificationRepository,
            INotificationAssessmentRepository assessmentRepository,
            INotificationAssessmentDatesSummaryRepository datesSummaryRepository,
            INotificationAssessmentDecisionRepository decisionRepository,
            IFacilityRepository facilityRepository,
            IConsultationRepository consultationRepository,
            IMapper mapper)
        {
            this.notificationRepository = notificationRepository;
            this.assessmentRepository = assessmentRepository;
            this.datesSummaryRepository = datesSummaryRepository;
            this.decisionRepository = decisionRepository;
            this.facilityRepository = facilityRepository;
            this.mapper = mapper;
            this.consultationRepository = consultationRepository;
        }

        public async Task<KeyDatesSummaryData> HandleAsync(GetKeyDatesSummaryInformation message)
        {
            var notification = await notificationRepository.GetById(message.NotificationId);

            var assessment = await assessmentRepository.GetByNotificationId(message.NotificationId);

            var dates = await datesSummaryRepository.GetById(message.NotificationId);

            var decision = await decisionRepository.GetByNotificationId(message.NotificationId);

            var facilityCollection = await facilityRepository.GetByNotificationId(message.NotificationId);

            var consultation = await consultationRepository.GetByNotificationId(message.NotificationId);

            return new KeyDatesSummaryData
            {
                CompetentAuthority = notification.CompetentAuthority,
                IsLocalAreaSet = consultation != null && consultation.LocalAreaId.HasValue,
                Dates = mapper.Map<NotificationDatesData>(dates),
                DecisionHistory = decision,
                IsInterim = facilityCollection.IsInterim
            };
        }
    }
}
