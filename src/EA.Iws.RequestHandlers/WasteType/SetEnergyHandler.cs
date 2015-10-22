namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetEnergyHandler : IRequestHandler<SetEnergy, Guid>
    {
        private readonly IwsContext context;

        public SetEnergyHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetEnergy command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            notification.SetEnergy(command.EnergyInformation);
            await context.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}