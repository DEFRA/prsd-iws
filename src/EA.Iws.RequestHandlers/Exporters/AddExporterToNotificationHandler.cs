namespace EA.Iws.RequestHandlers.Exporters
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Exporters;

    public class AddExporterToNotificationHandler : IRequestHandler<AddExporterToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddExporterToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddExporterToNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Exporter.Address.CountryId);

            var address = ValueObjectInitializer.CreateAddress(message.Exporter.Address, country.Name);

            var contact = ValueObjectInitializer.CreateContact(message.Exporter.Contact);

            var business = ValueObjectInitializer.CreateBusiness(message.Exporter.Business);

            var exporter = new Exporter(business, address, contact);

            var notification = await context.NotificationApplications.FindAsync(message.Exporter.NotificationId);
            notification.AddExporter(exporter);

            await context.SaveChangesAsync();

            return exporter.Id;
        }
    }
}