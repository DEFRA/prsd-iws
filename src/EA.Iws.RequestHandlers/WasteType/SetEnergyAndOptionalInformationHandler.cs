namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetEnergyAndOptionalInformationHandler : IRequestHandler<SetEnergyAndOptionalInformation, Guid>
    {
        private readonly IwsContext context;

        public SetEnergyAndOptionalInformationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetEnergyAndOptionalInformation command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            notification.SetEnergyAndOptionalInformation(command.EnergyInformation, command.OptionalInformation, command.HasAnnex);
            await context.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}