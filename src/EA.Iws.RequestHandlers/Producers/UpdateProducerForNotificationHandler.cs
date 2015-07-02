namespace EA.Iws.RequestHandlers.Producers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class UpdateProducerForNotificationHandler : IRequestHandler<UpdateProducerForNotification, Guid>
    {
        private readonly IwsContext context;

        public UpdateProducerForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(UpdateProducerForNotification message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var notification =
                await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            var business = ProducerBusiness.CreateProducerBusiness(message.Business.Name, 
                BusinessType.FromBusinessType(message.Business.BusinessType), 
                message.Business.RegistrationNumber, 
                message.Business.OtherDescription);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            var producer = notification.GetProducer(message.ProducerId);

            producer.Address = address;
            producer.Business = business;
            producer.Contact = contact;

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}