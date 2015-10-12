namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;

    internal class GetDisposalCostHandler : IRequestHandler<GetDisposalCost, ValuePerWeightData>
    {
        private readonly IWasteDisposalRepository wasteDisposalRepository;
        private readonly IMap<ValuePerWeight, ValuePerWeightData> wasteDisposalDataMap;

        public GetDisposalCostHandler(IWasteDisposalRepository wasteDisposalRepository, IMap<ValuePerWeight, ValuePerWeightData> wasteDisposalDataMap)
        {
            this.wasteDisposalRepository = wasteDisposalRepository;
            this.wasteDisposalDataMap = wasteDisposalDataMap;
        }

        public async Task<ValuePerWeightData> HandleAsync(GetDisposalCost message)
        {
            var disposalInfo = await wasteDisposalRepository.GetByNotificationId(message.NotificationId);

            if (disposalInfo != null)
            {
                return wasteDisposalDataMap.Map(disposalInfo.Cost);
            }

            return null;
        }
   }
}
