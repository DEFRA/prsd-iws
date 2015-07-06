namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    internal class SetWoodTypeDescriptionHandler : IRequestHandler<SetWoodTypeDescription, Guid>
    {
        private readonly IwsContext db;
        public SetWoodTypeDescriptionHandler(IwsContext db)
        {
            this.db = db;
        }
        public async Task<Guid> HandleAsync(SetWoodTypeDescription command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            notification.SetWoodTypeDescription(command.WoodTypeDescription);
            await db.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}