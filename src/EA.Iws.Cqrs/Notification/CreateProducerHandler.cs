namespace EA.Iws.Cqrs.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mediator;

    internal class CreateProducerHandler : IRequestHandler<CreateProducer, Guid>
    {
        private readonly IwsContext db;

        public CreateProducerHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(CreateProducer command)
        {
            var country = await db.Countries.SingleAsync(c => c.Id == new Guid(command.Producer.Address.Country));

            var notificationId = command.Producer.NotificationId;

            var address = new Address(command.Producer.Address.Building,
                command.Producer.Address.StreetOrSuburb,
                command.Producer.Address.TownOrCity,
                command.Producer.Address.PostalCode,
                country,
                command.Producer.Address.Address2);

            var contact = new Contact(command.Producer.Contact.FirstName,
                command.Producer.Contact.LastName,
                command.Producer.Contact.Telephone,
                command.Producer.Contact.Email,
                command.Producer.Contact.Fax);

            var producer = new Producer(command.Producer.Name,
                address,
                contact,
                notificationId,
                command.Producer.IsSiteOfExport,
                command.Producer.Type,
                command.Producer.CompaniesHouseNumber,
                command.Producer.RegistrationNumber1,
                command.Producer.RegistrationNumber2);

            db.Producers.Add(producer);
            await db.SaveChangesAsync();

            return producer.Id;
        }
    }
}