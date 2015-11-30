namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class GetNotificationLocalAreaIdHandler : IRequestHandler<GetNotificationLocalAreaId, Guid>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public GetNotificationLocalAreaIdHandler(INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<Guid> HandleAsync(GetNotificationLocalAreaId message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);
            
            return assessment.LocalAreaId.GetValueOrDefault(); 
        }
    }
}
