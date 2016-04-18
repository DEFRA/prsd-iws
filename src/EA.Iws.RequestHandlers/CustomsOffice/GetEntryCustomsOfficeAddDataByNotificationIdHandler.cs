namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using Core.CustomsOffice;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetEntryCustomsOfficeAddDataByNotificationIdHandler :
        IRequestHandler<GetEntryCustomsOfficeAddDataByNotificationId, EntryCustomsOfficeAddData>
    {
        private readonly ITransportRouteRepository repository;
        private readonly IMap<TransportRoute, EntryCustomsOfficeAddData> customsOfficeEntryMap;

        public GetEntryCustomsOfficeAddDataByNotificationIdHandler(ITransportRouteRepository repository,
            IMap<TransportRoute, EntryCustomsOfficeAddData> customsOfficeEntryMap)
        {
            this.repository = repository;
            this.customsOfficeEntryMap = customsOfficeEntryMap;
        }

        public async Task<EntryCustomsOfficeAddData> HandleAsync(GetEntryCustomsOfficeAddDataByNotificationId message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);

            return customsOfficeEntryMap.Map(transportRoute);
        }
    }
}