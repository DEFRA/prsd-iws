namespace EA.Iws.RequestHandlers.Producers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class AddProducerToNotificationHandler : IRequestHandler<AddProducerToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddProducerToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddProducerToNotification command)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == command.Address.CountryId);

            var business = ValueObjectInitializer.CreateBusiness(command.Business);
            var address = ValueObjectInitializer.CreateAddress(command.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(command.Contact);

            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            var producer = notification.AddProducer(business, address, contact);

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}