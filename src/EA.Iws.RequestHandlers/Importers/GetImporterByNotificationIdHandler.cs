namespace EA.Iws.RequestHandlers.Importers
{
    using System.Threading.Tasks;
    using Core.Importer;
    using Domain.NotificationApplication.Importer;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Importer;

    internal class GetImporterByNotificationIdHandler : IRequestHandler<GetImporterByNotificationId, ImporterData>
    {
        private readonly IMap<Importer, ImporterData> mapper;
        private readonly IImporterRepository repository;

        public GetImporterByNotificationIdHandler(IImporterRepository repository, IMap<Importer, ImporterData> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ImporterData> HandleAsync(GetImporterByNotificationId message)
        {
            var importer = await repository.GetImporterOrDefaultByNotificationId(message.NotificationId);

            return mapper.Map(importer);
        }
    }
}