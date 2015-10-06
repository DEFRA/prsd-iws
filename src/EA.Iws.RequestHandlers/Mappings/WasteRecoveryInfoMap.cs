namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mapper;
    using Requests.WasteRecovery;
    using WasteRecoveryOverview = Requests.Notification.Overview.WasteRecoveryOverview;

    internal class WasteRecoveryInfoMap : IMap<WasteRecovery, WasteRecoveryOverview>
    {
        private readonly IMap<ValuePerWeight, ValuePerWeightData> valueMapper;

        public WasteRecoveryInfoMap(IMap<ValuePerWeight, ValuePerWeightData> valueMapper)
        {
            this.valueMapper = valueMapper;
        }

        public WasteRecoveryOverview Map(WasteRecovery wasteRecovery)
        {
            WasteRecoveryOverview data = null;

            if (wasteRecovery != null)
            {
                data = new WasteRecoveryOverview
                {
                    NotificationId = wasteRecovery.NotificationId,
                    PercentageRecoverable = wasteRecovery.PercentageRecoverable.Value,
                    EstimatedValue = valueMapper.Map(wasteRecovery.EstimatedValue),
                    RecoveryCost = valueMapper.Map(wasteRecovery.RecoveryCost)
                };
            }

            return data; 
        }
    }
}
