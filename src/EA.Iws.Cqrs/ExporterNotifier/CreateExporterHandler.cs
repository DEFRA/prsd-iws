namespace EA.Iws.Cqrs.ExporterNotifier
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using Core.Domain;
    using DataAccess;
    using Domain;

    public class CreateExporterHandler : ICommandHandler<CreateExporter>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IQueryBus queryBus;

        public CreateExporterHandler(IwsContext context, IUserContext userContext, IQueryBus queryBus)
        {
            this.context = context;
            this.userContext = userContext;
            this.queryBus = queryBus;
        }

        public async Task HandleAsync(CreateExporter command)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == command.CountryId);

            var address = new Address(command.Building, command.Address1, command.City, command.PostCode,
                country, command.Address2);

            var contact = new Contact(command.FirstName, command.LastName, command.Phone, command.Email, command.Fax);

            var exporter = new Exporter(command.Name, command.Type, address, contact, command.CompanyHouseNumber, command.RegistrationNumber1, command.RegistrationNumber2);

            context.Exporters.Add(exporter);

            var notification = await context.NotificationApplications.FindAsync(command.NotificationId);
            notification.AddExporterToNotification(exporter);

            await context.SaveChangesAsync();
        }
    }
}
