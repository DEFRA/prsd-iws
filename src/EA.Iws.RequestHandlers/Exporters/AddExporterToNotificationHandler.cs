namespace EA.Iws.RequestHandlers.Exporters
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Exporters;

    internal class AddExporterToNotificationHandler : IRequestHandler<AddExporterToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddExporterToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddExporterToNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);
            var business = ValueObjectInitializer.CreateBusiness(message.Business);

            notification.AddExporter(business, address, contact);

            await context.SaveChangesAsync();

            return notification.Exporter.Id;
        }
    }
}