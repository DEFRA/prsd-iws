namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetAcknowlegedDateHandler : IRequestHandler<SetAcknowlegedDate, bool>
    {
        private readonly IImportNotificationAssessmentRepository repository;
        private readonly ImportNotificationContext context;

        public SetAcknowlegedDateHandler(IImportNotificationAssessmentRepository repository, 
            ImportNotificationContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetAcknowlegedDate message)
        {
            var assessment = await repository.GetByNotification(message.ImportNotificationId);

            assessment.Acknowledge(new DateTimeOffset(message.Date, TimeSpan.Zero));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
