namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetNotificationLocalAreaIdHandler : IRequestHandler<SetNotificationLocalAreaId, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public SetNotificationLocalAreaIdHandler(IwsContext context,
            INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.context = context;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<Guid> HandleAsync(SetNotificationLocalAreaId message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);

            assessment.SetLocalAreaId(message.LocalAreaId);

            await context.SaveChangesAsync();

            return message.LocalAreaId;
        }
    }
}
