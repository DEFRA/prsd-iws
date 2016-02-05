namespace EA.Iws.RequestHandlers.OperationCodes
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.OperationCodes;

    internal class AddRecoveryCodesHandler : IRequestHandler<AddRecoveryCodes, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationRepository notificationRepository;

        public AddRecoveryCodesHandler(IwsContext context, INotificationApplicationRepository notificationRepository)
        {
            this.context = context;
            this.notificationRepository = notificationRepository;
        }

        public async Task<Guid> HandleAsync(AddRecoveryCodes command)
        {
            var notification = await notificationRepository.GetById(command.NotificationId);

            notification.SetOperationCodes(command.RecoveryCodes);

            await context.SaveChangesAsync();

            return command.NotificationId;
        }
    }
}