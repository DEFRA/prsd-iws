namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetExportNotificationLocalAreaIdHandler : IRequestHandler<SetExportNotificationLocalAreaId, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public SetExportNotificationLocalAreaIdHandler(IwsContext context,
            INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.context = context;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<Guid> HandleAsync(SetExportNotificationLocalAreaId message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);

            assessment.SetLocalAreaId(message.LocalAreaId);

            await context.SaveChangesAsync();

            return message.LocalAreaId;
        }
    }
}
