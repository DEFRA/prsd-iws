namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetNotificationCompletedDateHandler : IRequestHandler<SetNotificationCompletedDate, bool>
    {
        private readonly IImportNotificationAssessmentRepository repository;
        private readonly ImportNotificationContext context;

        public SetNotificationCompletedDateHandler(IImportNotificationAssessmentRepository repository, 
            ImportNotificationContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetNotificationCompletedDate message)
        {
            var assessment = await repository.GetByNotification(message.ImportNotificationId);

            assessment.CompleteNotification(message.Date);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
