namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetExitCustomsOfficeAddDataByNotificationIdHandler :
        IRequestHandler<GetExitCustomsOfficeAddDataByNotificationId, ExitCustomsOfficeAddData>
    {
        private readonly ITransportRouteRepository repository;
        private readonly IMap<TransportRoute, ExitCustomsOfficeAddData> customsOfficeExitMap;

        public GetExitCustomsOfficeAddDataByNotificationIdHandler(ITransportRouteRepository repository,
            IMap<TransportRoute, ExitCustomsOfficeAddData> customsOfficeExitMap)
        {
            this.repository = repository;
            this.customsOfficeExitMap = customsOfficeExitMap;
        }

        public async Task<ExitCustomsOfficeAddData> HandleAsync(GetExitCustomsOfficeAddDataByNotificationId message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);

            return customsOfficeExitMap.Map(transportRoute);
        }
    }
}