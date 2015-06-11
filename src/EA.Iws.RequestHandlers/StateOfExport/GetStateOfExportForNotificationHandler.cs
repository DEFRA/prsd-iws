namespace EA.Iws.RequestHandlers.StateOfExport
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.StateOfExport;

    internal class GetStateOfExportForNotificationHandler : IRequestHandler<GetStateOfExportForNotification, StateOfExportData>
    {
        private readonly IwsContext context;
        private readonly IMap<StateOfExport, StateOfExportData> mapper;

        public GetStateOfExportForNotificationHandler(IwsContext context, IMap<StateOfExport, StateOfExportData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<StateOfExportData> HandleAsync(GetStateOfExportForNotification message)
        {
            var stateOfExport =
                await context.NotificationApplications.Where(n => n.Id == message.NotificationId).Select(n => n.StateOfExport).SingleAsync();

            return mapper.Map(stateOfExport);
        }
    }
}
