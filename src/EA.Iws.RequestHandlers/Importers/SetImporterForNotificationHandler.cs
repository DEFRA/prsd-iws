namespace EA.Iws.RequestHandlers.Importers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Importer;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Importer;

    internal class SetImporterForNotificationHandler : IRequestHandler<SetImporterForNotification, Guid>
    {
        private readonly IImporterRepository repository;
        private readonly IwsContext context;

        public SetImporterForNotificationHandler(IImporterRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetImporterForNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var business = ValueObjectInitializer.CreateBusiness(message.Business);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var importer = await repository.GetImporterOrDefaultByNotificationId(message.NotificationId);

            if (importer == null)
            {
                importer = new Importer(message.NotificationId, address, business, contact);
                repository.Add(importer);
            }
            else
            {
                importer.Update(address, business, contact);
            }

            await context.SaveChangesAsync();

            return importer.Id;
        }
    }
}