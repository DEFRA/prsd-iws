namespace EA.Iws.RequestHandlers.Importers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
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
            var country = await context.Countries.SingleAsync(c => c.Id == message.Importer.Address.CountryId);

            var address = ValueObjectInitializer.CreateAddress(message.Importer.Address, country.Name);

            var contact = ValueObjectInitializer.CreateContact(message.Importer.Contact);

            var business = ValueObjectInitializer.CreateBusiness(message.Importer.Business);

            var importer = new Importer(business, address, contact);

            var notification = await context.NotificationApplications.FindAsync(message.Importer.NotificationId);
            notification.AddImporter(importer);

            await context.SaveChangesAsync();

            return importer.Id;
        }
    }
}
