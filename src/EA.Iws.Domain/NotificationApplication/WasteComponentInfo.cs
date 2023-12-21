namespace EA.Iws.Domain.NotificationApplication
{
    using EA.Iws.Core.WasteComponentType;
    using EA.Prsd.Core.Domain;

    public class WasteComponentInfo : Entity
    {
        public WasteComponentType WasteComponentType { get; private set; }

        protected WasteComponentInfo()
        {
        }

        private WasteComponentInfo(WasteComponentType wasteComponentType)
        {
            WasteComponentType = wasteComponentType;
        }

        public static WasteComponentInfo CreateWasteComponentInfo(WasteComponentType wasteComponentType)
        {
            return new WasteComponentInfo(wasteComponentType);
        }
    }
}
