namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationConsent;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class ConsentNotificationApplicationHandler : IRequestHandler<ConsentNotificationApplication, bool>
    {
        private readonly ConsentNotification consentNotification;
        private readonly IwsContext context;

        public ConsentNotificationApplicationHandler(ConsentNotification consentNotification,
            IwsContext context)
        {
            this.consentNotification = consentNotification;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ConsentNotificationApplication message)
        {
            await
                consentNotification.Consent(message.NotificationId, new DateRange(message.ConsentFrom, message.ConsentTo),
                    message.ConsentConditions);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
