namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Threading.Tasks;
    using Core.Carriers;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class GetCarrierForNotificationHandler : IRequestHandler<GetCarrierForNotification, CarrierData>
    {
        private readonly ICarrierRepository repository;
        private readonly IMapWithParentObjectId<Carrier, CarrierData> mapper;

        public GetCarrierForNotificationHandler(ICarrierRepository repository,
            IMapWithParentObjectId<Carrier, CarrierData> mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<CarrierData> HandleAsync(GetCarrierForNotification message)
        {
            var carriers = await repository.GetByNotificationId(message.NotificationId);

            var carrier = carriers.GetCarrier(message.CarrierId);

            return mapper.Map(carrier, message.NotificationId);
        }
    }
}