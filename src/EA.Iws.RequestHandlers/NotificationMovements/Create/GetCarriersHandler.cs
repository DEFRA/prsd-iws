namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Carriers;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GetCarriersHandler : IRequestHandler<GetCarriers, IList<CarrierData>>
    {
        private readonly IMapper mapper;
        private readonly INotificationApplicationRepository repository;

        public GetCarriersHandler(INotificationApplicationRepository repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<CarrierData>> HandleAsync(GetCarriers message)
        {
            var notification = await repository.GetById(message.NotificationId);

            return mapper.Map<IList<CarrierData>>(notification);
        }
    }
}