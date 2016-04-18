namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.NotificationConsent;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GetShipmentDatesHandler : IRequestHandler<GetShipmentDates, ShipmentDates>
    {
        private readonly IMapper mapper;
        private readonly INotificationConsentRepository consentRepository;

        public GetShipmentDatesHandler(INotificationConsentRepository consentRepository,
            IMapper mapper)
        {
            this.consentRepository = consentRepository;
            this.mapper = mapper;
        }

        public async Task<ShipmentDates> HandleAsync(GetShipmentDates message)
        {
            var consent = await consentRepository.GetByNotificationId(message.NotificationId);

            return mapper.Map<ShipmentDates>(consent.ConsentRange);
        }
    }
}