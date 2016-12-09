namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class UpdateInterimStatusHandler : IRequestHandler<UpdateInterimStatus, bool>
    {
        private readonly IInterimStatusRepository repository;
        private readonly ImportNotificationContext context;

        public UpdateInterimStatusHandler(IInterimStatusRepository repository, ImportNotificationContext context)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(UpdateInterimStatus message)
        {
            await repository.UpdateStatus(message.ImportNotificationId, message.IsInterim);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
