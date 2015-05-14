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
            var country = await context.Countries.SingleAsync(c => c.Id == command.CountryId);

            var address = new Address(command.Building,
                command.Address1,
                command.TownOrCity,
                command.PostalCode,
                country.Name,
                command.Address2);

            var contact = new Contact(command.FirstName,
                command.LastName,
                command.Phone,
                command.Email,
                command.Fax);

            var business = new Business(command.Name,
                command.Type,
                command.RegistrationNumber,
                command.AdditionalRegistrationNumber);

            var producer = new Producer(business,
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