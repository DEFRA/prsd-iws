namespace EA.Iws.RequestHandlers.Importers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Importer;

    internal class UpdateImporterForNotificationHandler : IRequestHandler<UpdateImporterForNotification, Guid>
    {
        private readonly IwsContext context;

        public UpdateImporterForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(UpdateImporterForNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);
            var notification = await context.NotificationApplications.FindAsync(message.NotificationId);

            notification.Importer.Address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            notification.Importer.Contact = ValueObjectInitializer.CreateContact(message.Contact);
            notification.Importer.Business = ValueObjectInitializer.CreateBusiness(message.Business);

            await context.SaveChangesAsync();

            return notification.Importer.Id;
        }
    }
}