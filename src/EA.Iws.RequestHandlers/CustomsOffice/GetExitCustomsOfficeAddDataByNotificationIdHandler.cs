namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetExitCustomsOfficeAddDataByNotificationIdHandler :
        IRequestHandler<GetExitCustomsOfficeAddDataByNotificationId, ExitCustomsOfficeAddData>
    {
        private readonly IwsContext context;
        private readonly IMap<TransportRoute, ExitCustomsOfficeAddData> customsOfficeExitMap;

        public GetExitCustomsOfficeAddDataByNotificationIdHandler(IwsContext context,
            IMap<TransportRoute, ExitCustomsOfficeAddData> customsOfficeExitMap)
        {
            this.context = context;
            this.customsOfficeExitMap = customsOfficeExitMap;
        }

        public async Task<ExitCustomsOfficeAddData> HandleAsync(GetExitCustomsOfficeAddDataByNotificationId message)
        {
            await context.CheckNotificationAccess(message.Id);

            var transportRoute = await context.TransportRoutes.SingleAsync(p => p.NotificationId == message.Id);

            return customsOfficeExitMap.Map(transportRoute);
        }
    }
}