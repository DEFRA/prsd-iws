namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetWasteGenerationProcessHandler : IRequestHandler<SetWasteGenerationProcess, Guid>
    {
        private readonly IwsContext context;

        public SetWasteGenerationProcessHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetWasteGenerationProcess command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.AddWasteGenerationProcess(command.Process, command.IsDocumentAttached);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}