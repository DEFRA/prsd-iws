namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class CreateProducerHandler : IRequestHandler<CreateProducer, Guid>
    {
        private readonly IwsContext db;

        public CreateProducerHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(CreateProducer command)
        {
            var country = await db.Countries.SingleAsync(c => c.Id == command.Address.CountryId);

            var notificationId = command.NotificationId;

            var address = new Address(command.Address.Building,
                command.Address.StreetOrSuburb,
                command.Address.TownOrCity,
                command.Address.PostalCode,
                country,
                command.Address.Address2);

            var contact = new Contact(command.Contact.FirstName,
                command.Contact.LastName,
                command.Contact.Telephone,
                command.Contact.Email,
                command.Contact.Fax);

            var producer = new Producer(command.Name,
                address,
                contact,
                notificationId,
                command.IsSiteOfExport,
                command.Type,
                command.CompaniesHouseNumber,
                command.RegistrationNumber1,
                command.RegistrationNumber2);

            db.Producers.Add(producer);
            await db.SaveChangesAsync();

            return producer.Id;
        }
    }
}