namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportNotificationConsentedDateHandler : IRequestHandler<GetImportNotificationConsentedDate, DateTime?>
    {
        private readonly IImportNotificationAssessmentRepository repository;

        public GetImportNotificationConsentedDateHandler(IImportNotificationAssessmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DateTime?> HandleAsync(GetImportNotificationConsentedDate message)
        {
            return await repository.GetConsentedDate(message.NotificationId);
        }
    }
}
