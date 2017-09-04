namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Importer;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetImporterContactHandler : IRequestHandler<SetImporterContact, Unit>
    {
        private readonly IwsContext context;
        private readonly IImporterRepository repository;

        public SetImporterContactHandler(IImporterRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetImporterContact message)
        {
            var importer = await repository.GetByNotificationId(message.NotificationId);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);
            importer.UpdateContact(contact);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}