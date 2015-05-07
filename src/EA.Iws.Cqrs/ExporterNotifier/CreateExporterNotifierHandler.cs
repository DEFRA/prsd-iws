namespace EA.Iws.Cqrs.ExporterNotifier
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using Core.Domain;
    using DataAccess;
    using Domain;

    public class CreateExporterNotifierHandler : ICommandHandler<CreateExporter>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IQueryBus queryBus;

        public CreateExporterNotifierHandler(IwsContext context, IUserContext userContext, IQueryBus queryBus)
        {
            this.context = context;
            this.userContext = userContext;
            this.queryBus = queryBus;
        }

        public async Task HandleAsync(CreateExporter command)
        {
            var country = await context.Countries.SingleAsync(c => c.Name.Equals(command.CountryName));

            var address = new Address(command.Building, "Address 1", string.Empty, string.Empty, country);

            var contact = new Contact(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            var exporter = new ExporterNotifier(command.Name, address, string.Empty,
                contact, command.RegistrationNumber);

            context.ExporterNotifiers.Add(exporter);

            var notification = await context.NotificationApplications.FindAsync(command.NotificationId);

            notification.AddExporterToNotification(exporter);

            await context.SaveChangesAsync();
        }
    }
}
