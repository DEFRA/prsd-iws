namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetOptionalInformationHandler : IRequestHandler<SetOptionalInformation, Guid>
    {
        private readonly IwsContext context;

        public SetOptionalInformationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetOptionalInformation command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            notification.SetOptionalInformation(command.OptionalInformation, command.HasAnnex);
            await context.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}