namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class GetNotificationLocalAreaIdHandler : IRequestHandler<GetExportNotificationLocalAreaId, Guid>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public GetNotificationLocalAreaIdHandler(INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<Guid> HandleAsync(GetExportNotificationLocalAreaId message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);
            
            return assessment.LocalAreaId.GetValueOrDefault(); 
        }
    }
}
