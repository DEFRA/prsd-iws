namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Exporter;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetExporterContactHandler : IRequestHandler<SetExporterContact, Unit>
    {
        private readonly IwsContext context;
        private readonly IExporterRepository repository;

        public SetExporterContactHandler(IExporterRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetExporterContact message)
        {
            var exporter = await repository.GetByNotificationId(message.NotificationId);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);
            exporter.UpdateContact(contact);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}