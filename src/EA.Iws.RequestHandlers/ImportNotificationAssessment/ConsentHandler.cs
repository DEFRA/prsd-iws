namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
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
                new DateTimeOffsetRange(new DateTimeOffset(message.From, TimeSpan.Zero),
                    new DateTimeOffset(message.To, TimeSpan.Zero)),
                message.Conditions,
                new DateTimeOffset(message.Date, TimeSpan.Zero));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
