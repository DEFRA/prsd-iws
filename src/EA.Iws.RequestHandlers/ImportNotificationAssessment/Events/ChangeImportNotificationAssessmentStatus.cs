namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Events
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Domain;

    internal class ChangeImportNotificationAssessmentStatus : IEventHandler<ImportNotificationStatusChangeEvent>
    {
        private readonly IUserContext userContext;
        private readonly ImportNotificationContext context;

        public ChangeImportNotificationAssessmentStatus(IUserContext userContext,
            ImportNotificationContext context)
        {
            this.userContext = userContext;
            this.context = context;
        }

        public async Task HandleAsync(ImportNotificationStatusChangeEvent @event)
        {
            var userId = userContext.UserId;

            @event.Assessment.AddStatusChangeRecord(new ImportNotificationStatusChange(@event.Source, @event.Destination, userId));

            await context.SaveChangesAsync();
        }
    }
}
