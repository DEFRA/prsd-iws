namespace EA.Iws.RequestHandlers.WasteType
{
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class SetOtherWasteAdditionalInformationHandler : IRequestHandler<SetOtherWasteAdditionalInformation, Guid>
    {
        private readonly IwsContext db;

        public SetOtherWasteAdditionalInformationHandler(IwsContext db)
        {
            this.db = db;
        }
        public async Task<Guid> HandleAsync(SetOtherWasteAdditionalInformation command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            notification.AddOtherWasteTypeAdditionalInformation(command.Description, command.HasAnnex);

            await db.SaveChangesAsync();

            return notification.WasteType.Id;
        }
    }
}