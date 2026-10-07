namespace EA.Iws.RequestHandlers.NotificationAssessment.Payment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Domain.Security;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment.Payment;

    internal class SaveGovUkPaySessionHandler : IRequestHandler<SaveGovUkPaySession, bool>
    {
        private readonly INotificationApplicationAuthorization authorization;
        private readonly IGovUkPaySessionRepository paySessionRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public SaveGovUkPaySessionHandler(INotificationApplicationAuthorization authorization,
            IGovUkPaySessionRepository paySessionRepository,
            IwsContext context,
            IUserContext userContext)
        {
            this.authorization = authorization;
            this.paySessionRepository = paySessionRepository;
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(SaveGovUkPaySession message)
        {
            await authorization.EnsureAccessAsync(message.NotificationId);

            var session = new GovUkPaySession(message.NotificationId,
                message.PaymentId,
                message.PaymentReference,
                message.SecureToken,
                message.Amount,
                userContext.UserId,
                SystemTime.UtcNow);

            paySessionRepository.Add(session);

            await context.SaveChangesAsync();

            return true;
        }
    }
}