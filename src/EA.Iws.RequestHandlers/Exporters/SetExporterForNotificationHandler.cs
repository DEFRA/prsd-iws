namespace EA.Iws.RequestHandlers.Exporters
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Exporter;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Exporters;

    internal class SetExporterForNotificationHandler : IRequestHandler<SetExporterForNotification, Guid>
    {
        private readonly IExporterRepository exporterRepository;
        private readonly IwsContext context;

        public SetExporterForNotificationHandler(IExporterRepository exporterRepository, IwsContext context)
        {
            this.exporterRepository = exporterRepository;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetExporterForNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);
            var business = ValueObjectInitializer.CreateBusiness(message.Business);

            var exporter = await exporterRepository.GetExporterOrDefaultByNotificationId(message.NotificationId);

            if (exporter == null)
            {
                exporter = new Exporter(message.NotificationId, address, business, contact);
                exporterRepository.Add(exporter);
            }
            else
            {
                exporter.Update(address, business, contact);
            }

            await context.SaveChangesAsync();

            return exporter.Id;
        }
    }
}