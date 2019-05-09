namespace EA.Iws.RequestHandlers.ImportNotification.Exporters
{
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using EA.Iws.Requests.ImportNotification.Exporters;
    using EA.Prsd.Core.Mediator;
    using Prsd.Core.Mapper;

    internal class GetExporterByImportNotificationIdHandler : IRequestHandler<GetExporterByImportNotificationId, Core.ImportNotification.Summary.Exporter>
    {
        private readonly IExporterRepository exporterRepository;
        private readonly IMapper mapper;
        public GetExporterByImportNotificationIdHandler(IExporterRepository exporterRepository, IMapper mapper)
        {
            this.exporterRepository = exporterRepository;
            this.mapper = mapper;
        }

        public async Task<Core.ImportNotification.Summary.Exporter> HandleAsync(GetExporterByImportNotificationId message)
        {
            var exporter = await exporterRepository.GetByNotificationId(message.ImportNotificationId);
            return mapper.Map<Core.ImportNotification.Summary.Exporter>(exporter);
        }
    }
}
