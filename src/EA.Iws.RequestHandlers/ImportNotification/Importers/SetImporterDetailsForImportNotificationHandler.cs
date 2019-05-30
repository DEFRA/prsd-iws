namespace EA.Iws.RequestHandlers.ImportNotification.Importers
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Importers;

    internal class SetImporterDetailsForImportNotificationHandler : IRequestHandler<SetImporterDetailsForImportNotification, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly IMapper mapper;
        private readonly IImporterRepository importerRepository;

        public SetImporterDetailsForImportNotificationHandler(IImporterRepository importerRepository, ImportNotificationContext context, IMapper mapper)
        {
            this.importerRepository = importerRepository;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Unit> HandleAsync(SetImporterDetailsForImportNotification message)
        {
            var importer = await importerRepository.GetByNotificationId(message.ImportNotificationId);
            var contact = mapper.Map<Contact>(message.ImporterDetails.Contact);
            importer.UpdateContactAndName(contact, message.ImporterDetails.Name);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
