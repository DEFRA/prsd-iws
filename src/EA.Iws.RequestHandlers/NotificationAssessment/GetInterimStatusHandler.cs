namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetInterimStatusHandler : IRequestHandler<GetInterimStatus, InterimStatus>
    {
        private readonly IFacilityRepository facilityRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public GetInterimStatusHandler(IFacilityRepository facilityRepository,
            INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.facilityRepository = facilityRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<InterimStatus> HandleAsync(GetInterimStatus message)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(message.NotificationId);
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);

            return new InterimStatus
            {
                IsInterim = facilityCollection.IsInterim,
                NotificationId = assessment.NotificationApplicationId,
                NotificationStatus = assessment.Status
            };
        }
    }
}