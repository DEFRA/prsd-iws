namespace EA.Iws.RequestHandlers.Producers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Producers;
    using Requests.Shared;

    internal class GetProducerForNotificationHandler : IRequestHandler<GetProducerForNotification, ProducerData>
    {
        private readonly IwsContext context;

        public GetProducerForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ProducerData> HandleAsync(GetProducerForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);
            var producer = notification.GetProducer(message.ProducerId);

            return new ProducerData
            {
                Id = producer.Id,
                Business = new BusinessData
                {
                    Name = producer.Business.Name,
                    EntityType = producer.Business.Type,
                    RegistrationNumber = producer.Business.RegistrationNumber,
                    AdditionalRegistrationNumber = producer.Business.AdditionalRegistrationNumber
                },
                Address =
                    new AddressData
                    {
                        Address2 = producer.Address.Address2,
                        CountryName = producer.Address.Country,
                        Building = producer.Address.Building,
                        PostalCode = producer.Address.PostalCode,
                        StreetOrSuburb = producer.Address.Address1,
                        TownOrCity = producer.Address.TownOrCity
                    },
                Contact = new ContactData
                {
                    Email = producer.Contact.Email,
                    FirstName = producer.Contact.FirstName,
                    LastName = producer.Contact.LastName,
                    Fax = producer.Contact.Fax,
                    Telephone = producer.Contact.Telephone
                },
                NotificationId = message.NotificationId,
                IsSiteOfExport = producer.IsSiteOfExport
            };
        }
    }
}