namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mapper;
    using Requests.Notification.Overview;
    using Requests.WasteRecovery;

    internal class WasteDisposalMap : IMap<WasteDisposal, WasteDisposalOverview>
    {
        private readonly IMap<ValuePerWeight, ValuePerWeightData> valueMapper;

        public WasteDisposalMap(IMap<ValuePerWeight, ValuePerWeightData> valueMapper)
        {
            this.valueMapper = valueMapper;
        }

        public WasteDisposalOverview Map(WasteDisposal source)
        {
            WasteDisposalOverview data = null;

            if (source != null)
            {
                data = new WasteDisposalOverview
                {
                    NotificationId = source.NotificationId,
                    DisposalMethod = source.Method,
                    DisposalCost = valueMapper.Map(source.Cost)
                };
            }

            return data;
        }
    }
}
