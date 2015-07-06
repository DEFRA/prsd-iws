namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    internal class SetEnergyAndOptionalInformationHandler : IRequestHandler<SetEnergyAndOptionalInformation, Guid>
    {
        private readonly IwsContext db;
        public SetEnergyAndOptionalInformationHandler(IwsContext db)
        {
            this.db = db;
        }
        public async Task<Guid> HandleAsync(SetEnergyAndOptionalInformation command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            notification.SetEnergyAndOptionalInformation(command.EnergyInformation, command.OptionalInformation);
            await db.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}