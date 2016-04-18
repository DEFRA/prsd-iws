namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetOtherWasteAdditionalInformationHandler : IRequestHandler<SetOtherWasteAdditionalInformation, Guid>
    {
        private readonly IwsContext context;

        public SetOtherWasteAdditionalInformationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetOtherWasteAdditionalInformation command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.AddOtherWasteTypeAdditionalInformation(command.Description, command.HasAnnex);

            await context.SaveChangesAsync();

            return notification.WasteType.Id;
        }
    }
}