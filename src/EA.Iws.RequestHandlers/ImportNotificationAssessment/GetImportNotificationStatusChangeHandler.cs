namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;

    public class GetImportNotificationStatusChangeHandler : IRequestHandler<GetImportNotificationStatusChange, ImportNotificationStatusChange>
    {
        private readonly IImportNotificationAssessmentRepository repository;

        public GetImportNotificationStatusChangeHandler(IImportNotificationAssessmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ImportNotificationStatusChange> HandleAsync(GetImportNotificationStatusChange message)
        {
            var previousStatusChange = await this.repository.GetPreviousStatusChangeByNotification(message.NotificationId);

            return previousStatusChange;
        }
    }
}
