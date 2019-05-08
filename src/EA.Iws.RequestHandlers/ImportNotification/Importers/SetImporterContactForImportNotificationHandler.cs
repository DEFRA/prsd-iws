namespace EA.Iws.RequestHandlers.ImportNotification.Importers
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Importers;

    internal class SetImporterContactForImportNotificationHandler : IRequestHandler<SetImporterContactForImportNotification, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly IMapper mapper;
        private readonly IImporterRepository importerRepository;

        public SetImporterContactForImportNotificationHandler(IImporterRepository importerRepository, ImportNotificationContext context, IMapper mapper)
        {
            this.importerRepository = importerRepository;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Unit> HandleAsync(SetImporterContactForImportNotification message)
        {
            var importer = await importerRepository.GetByNotificationId(message.ImportNotificationId);
            var contact = mapper.Map<Contact>(message.Contact);
            importer.UpdateContact(contact);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
