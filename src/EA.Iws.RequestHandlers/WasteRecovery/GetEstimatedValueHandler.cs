namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;

    internal class GetEstimatedValueHandler : IRequestHandler<GetEstimatedValue, ValuePerWeightData>
    {
        private readonly IWasteRecoveryRepository repository;
        private readonly IMap<ValuePerWeight, ValuePerWeightData> mapper;

        public GetEstimatedValueHandler(IWasteRecoveryRepository repository,
            IMap<ValuePerWeight, ValuePerWeightData> mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ValuePerWeightData> HandleAsync(GetEstimatedValue message)
        {
            var wasteRecovery = await repository.GetByNotificationId(message.NotificationId);

            if (wasteRecovery == null)
            {
                return null;
            }

            return mapper.Map(wasteRecovery.EstimatedValue);
        }
    }
}
