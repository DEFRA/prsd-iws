namespace EA.Iws.RequestHandlers.Producers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Notification;
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
            var country = await context.Countries.SingleAsync(c => c.Id == command.ProducerData.Address.CountryId);

            var address = ValueObjectInitializer.CreateAddress(command.ProducerData.Address, country.Name);

            var contact = ValueObjectInitializer.CreateContact(command.ProducerData.Contact);

            var business = ValueObjectInitializer.CreateBusiness(command.ProducerData.Business);

            var producer = new Producer(business,
                address,
                contact);

            var notification = await context.NotificationApplications.FindAsync(command.ProducerData.NotificationId);
            notification.AddProducer(producer);

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}