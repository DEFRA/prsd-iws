namespace EA.Iws.RequestHandlers.RecoveryInfo
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.RecoveryInfo;

    internal class SetRecoveryInfoProviderHandler : IRequestHandler<SetRecoveryInfoProvider, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationRepository repository;

        public SetRecoveryInfoProviderHandler(INotificationApplicationRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetRecoveryInfoProvider message)
        {
            var notification = await repository.GetById(message.NotificationId);

            notification.SetRecoveryInformationProvider(message.ProvidedBy);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
