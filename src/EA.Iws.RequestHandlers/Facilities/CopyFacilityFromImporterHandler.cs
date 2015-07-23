namespace EA.Iws.RequestHandlers.Facilities
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class CopyFacilityFromImporterHandler : IRequestHandler<CopyFacilityFromImporter, Guid>
    {
        private readonly IwsContext context;

        public CopyFacilityFromImporterHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(CopyFacilityFromImporter message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            var facility = notification.AddFacility(notification.Importer.Business, notification.Importer.Address,
                notification.Importer.Contact);

            await context.SaveChangesAsync();

            return facility.Id;
        }
    }
}