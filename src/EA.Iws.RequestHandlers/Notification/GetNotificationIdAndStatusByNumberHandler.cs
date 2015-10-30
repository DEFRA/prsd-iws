namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationIdAndStatusByNumberHandler : IRequestHandler<GetNotificationIdAndStatusByNumber, Tuple<Guid?, NotificationStatus?>>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public GetNotificationIdAndStatusByNumberHandler(INotificationApplicationRepository notificationApplicationRepository,
            INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<Tuple<Guid?, NotificationStatus?>> HandleAsync(GetNotificationIdAndStatusByNumber message)
        {
            var id = await notificationApplicationRepository.GetIdOrDefault(message.Number);

            if (!id.HasValue)
            {
                return new Tuple<Guid?, NotificationStatus?>(null, null);
            }

            var notificationAssessment = await notificationAssessmentRepository.GetByNotificationId(id.Value);

            return new Tuple<Guid?, NotificationStatus?>(id, notificationAssessment.Status);
        }
    }
}
