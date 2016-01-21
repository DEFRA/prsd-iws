namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.ImportNotificationAssessment.Consent;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class ConsentHandler : IRequestHandler<Consent, bool>
    {
        private readonly ConsentImportNotification consentImportNotification;
        private readonly ImportNotificationContext context;

        public ConsentHandler(ConsentImportNotification consentImportNotification,
            ImportNotificationContext context)
        {
            this.consentImportNotification = consentImportNotification;
            this.context = context;
        }

        public async Task<bool> HandleAsync(Consent message)
        {
            await consentImportNotification.Consent(message.ImportNotificationId,
                new DateRange(message.From, message.To),
                message.Conditions,
                message.Date);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
