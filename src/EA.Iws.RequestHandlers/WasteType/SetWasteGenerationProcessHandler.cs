namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetWasteGenerationProcessHandler : IRequestHandler<SetWasteGenerationProcess, Guid>
    {
        private readonly IwsContext db;

        public SetWasteGenerationProcessHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(SetWasteGenerationProcess command)
        {
            var notification = await db.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == command.NotificationId);

            notification.AddWasteGenerationProcess(command.Process, command.IsDocumentAttached);

            await db.SaveChangesAsync();

            return notification.WasteType.Id;
        }
    }
}