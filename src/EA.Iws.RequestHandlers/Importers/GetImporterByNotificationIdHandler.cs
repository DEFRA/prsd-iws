namespace EA.Iws.RequestHandlers.Importers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Mappings;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Importer;

    internal class GetImporterByNotificationIdHandler : IRequestHandler<GetImporterByNotificationId, ImporterData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, ImporterData> mapper;

        public GetImporterByNotificationIdHandler(IwsContext context, IMap<NotificationApplication, ImporterData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ImporterData> HandleAsync(GetImporterByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return mapper.Map(notification);
        }
    }
}