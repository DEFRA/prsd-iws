namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetEntryCustomsOfficeAddDataByNotificationIdHandler :
        IRequestHandler<GetEntryCustomsOfficeAddDataByNotificationId, EntryCustomsOfficeAddData>
    {
        private readonly IwsContext context;
        private readonly IMap<TransportRoute, EntryCustomsOfficeAddData> customsOfficeEntryMap;

        public GetEntryCustomsOfficeAddDataByNotificationIdHandler(IwsContext context,
            IMap<TransportRoute, EntryCustomsOfficeAddData> customsOfficeEntryMap)
        {
            this.context = context;
            this.customsOfficeEntryMap = customsOfficeEntryMap;
        }

        public async Task<EntryCustomsOfficeAddData> HandleAsync(GetEntryCustomsOfficeAddDataByNotificationId message)
        {
            await context.CheckNotificationAccess(message.Id);

            var transportRoute = await context.TransportRoutes.SingleAsync(p => p.NotificationId == message.Id);

            return customsOfficeEntryMap.Map(transportRoute);
        }
    }
}