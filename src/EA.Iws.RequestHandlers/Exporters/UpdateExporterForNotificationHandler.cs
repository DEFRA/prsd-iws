namespace EA.Iws.RequestHandlers.Exporters
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Exporters;

    internal class UpdateExporterForNotificationHandler : IRequestHandler<UpdateExporterForNotification, Guid>
    {
        private readonly IwsContext context;

        public UpdateExporterForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(UpdateExporterForNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);
            var notification = await context.NotificationApplications.FindAsync(message.NotificationId);
            
            notification.Exporter.Address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            notification.Exporter.Contact = ValueObjectInitializer.CreateContact(message.Contact);
            notification.Exporter.Business = ValueObjectInitializer.CreateBusiness(message.Business);

            await context.SaveChangesAsync();

            return notification.Exporter.Id;
        }
    }
}