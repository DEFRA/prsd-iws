namespace EA.Iws.RequestHandlers.Importers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Importer;

    internal class AddImporterToNotificationHandler : IRequestHandler<AddImporterToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddImporterToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddImporterToNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var business = ValueObjectInitializer.CreateBusiness(message.Business);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);
            notification.AddImporter(business, address, contact);

            await context.SaveChangesAsync();

            return notification.Importer.Id;
        }
    }
}