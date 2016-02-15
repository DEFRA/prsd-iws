namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetImportNotificationLocalAreaIdHandler : IRequestHandler<SetImportNotificationLocalAreaId, Guid>
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationAssessmentRepository repository;

        public SetImportNotificationLocalAreaIdHandler(ImportNotificationContext context, IImportNotificationAssessmentRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(SetImportNotificationLocalAreaId message)
        {
            var assessment = await repository.GetByNotification(message.NotificationId);

            assessment.SetLocalAreaId(message.LocalAreaId);

            await context.SaveChangesAsync();

            return message.LocalAreaId;
        }
    }
}
