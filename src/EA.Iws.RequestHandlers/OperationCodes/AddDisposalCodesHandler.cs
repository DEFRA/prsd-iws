namespace EA.Iws.RequestHandlers.OperationCodes
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.OperationCodes;

    internal class AddDisposalCodesHandler : IRequestHandler<AddDisposalCodes, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationRepository noticationRepository;

        public AddDisposalCodesHandler(IwsContext context, INotificationApplicationRepository noticationRepository)
        {
            this.context = context;
            this.noticationRepository = noticationRepository;
        }

        public async Task<Guid> HandleAsync(AddDisposalCodes command)
        {
            var notification = await noticationRepository.GetById(command.NotificationId);

            notification.SetOperationCodes(command.DisposalCodes);

            await context.SaveChangesAsync();

            return command.NotificationId;
        }
    }
}