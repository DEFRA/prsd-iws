namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetWoodTypeDescriptionHandler : IRequestHandler<SetWoodTypeDescription, Guid>
    {
        private readonly IwsContext context;

        public SetWoodTypeDescriptionHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetWoodTypeDescription command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            notification.SetWoodTypeDescription(command.WoodTypeDescription);
            await context.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}