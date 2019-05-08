namespace EA.Iws.RequestHandlers.ImportNotification.Exporters
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Exporters;

    internal class SetExporterContactForImportNotificationHandler : IRequestHandler<SetExporterContactForImportNotification, Unit>
    {
        private readonly IExporterRepository exporterRepository;
        private readonly ImportNotificationContext context;
        private readonly IMapper mapper;

        public SetExporterContactForImportNotificationHandler(IExporterRepository exporterRepository, ImportNotificationContext context, IMapper mapper)
        {
            this.exporterRepository = exporterRepository;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Unit> HandleAsync(SetExporterContactForImportNotification message)
        {
            var exporter = await exporterRepository.GetByNotificationId(message.ImportNotificationId);
            var contact = mapper.Map<Contact>(message.Contact);
            exporter.UpdateContact(contact);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
