namespace EA.Iws.Cqrs.Notification
{
    using System;
    using System.Data.Entity;
    using Api.Client.Entities;
    using Core.Cqrs;
    using DataAccess;
    using Domain;
    using Domain.Notification;

    internal class CreateProducerHandler : ICommandHandler<CreateProducer>
    {
        private readonly IwsContext db;

        public CreateProducerHandler(IwsContext db)
        {
            this.db = db;
        }

        public async System.Threading.Tasks.Task HandleAsync(CreateProducer command)
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
        }
    }
}
