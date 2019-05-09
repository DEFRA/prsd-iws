namespace EA.Iws.RequestHandlers.ImportNotification.Importers
{
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using EA.Prsd.Core.Mediator;
    using Prsd.Core.Mapper;
    using Requests.ImportNotification.Importers;

    internal class GetImporterByImportNotificationIdHandler : IRequestHandler<GetImporterByImportNotificationId, Core.ImportNotification.Summary.Importer>
    {
        private readonly IImporterRepository importerRepository;
        private readonly IMapper mapper;

        public GetImporterByImportNotificationIdHandler(IImporterRepository importerRepository, IMapper mapper)
        {
            this.importerRepository = importerRepository;
            this.mapper = mapper;
        }

        public async Task<Core.ImportNotification.Summary.Importer> HandleAsync(GetImporterByImportNotificationId message)
        {
            var importer = await importerRepository.GetByNotificationId(message.ImportNotificationId);
            return mapper.Map<Core.ImportNotification.Summary.Importer>(importer);
        }
    }
}
