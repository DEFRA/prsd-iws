namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class ObjectToImportNotificationHandler : IRequestHandler<ObjectToImportNotification, bool>
    {
        private readonly IImportNotificationAssessmentRepository repository;
        private readonly ImportNotificationContext context;

        public ObjectToImportNotificationHandler(IImportNotificationAssessmentRepository repository, ImportNotificationContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ObjectToImportNotification message)
        {
            var assessment = await repository.GetByNotification(message.Id);

            assessment.Object(message.Date, message.ReasonsForObjection);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
