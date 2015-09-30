namespace EA.Iws.RequestHandlers.RecoveryInfo
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication.Recovery;
    using Prsd.Core.Mediator;
    using Requests.RecoveryInfo;

    public class AddRecoveryInfoToNotificationHandler : IRequestHandler<AddRecoveryInfoToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddRecoveryInfoToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public Task<Guid> HandleAsync(AddRecoveryInfoToNotification command)
        {
            throw new NotImplementedException();
        }
    }
}
