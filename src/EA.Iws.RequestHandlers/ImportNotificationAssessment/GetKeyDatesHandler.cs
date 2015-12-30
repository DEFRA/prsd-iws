namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    public class GetKeyDatesHandler : IRequestHandler<GetKeyDates, KeyDatesData>
    {
        private readonly IImportNotificationAssessmentRepository notificationAssessmentRepository;

        public GetKeyDatesHandler(IImportNotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<KeyDatesData> HandleAsync(GetKeyDates message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotification(message.ImportNotificationId);

            return new KeyDatesData
            {
                NotificationReceived =
                    assessment.Dates.NotificationReceivedDate.HasValue
                        ? assessment.Dates.NotificationReceivedDate.Value.DateTime
                        : (DateTime?)null
            };
        }
    }
}
