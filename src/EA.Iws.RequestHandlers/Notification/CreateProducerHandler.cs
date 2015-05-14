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
        private readonly IwsContext context;

        public CreateProducerHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(CreateProducer command)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == command.Address.CountryId);

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

            var businessNameAndType = new BusinessNameAndType(command.Name,
                command.Type,
                command.CompaniesHouseNumber,
                command.RegistrationNumber1,
                command.RegistrationNumber2);

            var producer = new Producer(businessNameAndType,
                address,
                contact,
                command.IsSiteOfExport);

            var notification = await context.NotificationApplications.FindAsync(command.NotificationId);
            notification.AddProducer(producer);

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}