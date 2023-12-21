namespace EA.Iws.RequestHandlers.Mappings
{
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.WasteComponentType;
    using EA.Prsd.Core.Mapper;
    using System;
    using System.Collections.Generic;

    internal class WasteComponentInfoMap : IMap<SetWasteComponentInfoForNotification, IEnumerable<WasteComponentInfo>>, 
                                           IMap<NotificationApplication, WasteComponentData>
    {
        public IEnumerable<WasteComponentInfo> Map(SetWasteComponentInfoForNotification source)
        {
            var wasteComponentInfos = new List<WasteComponentInfo>();

            foreach (var selectedComponentType in source.WasteComponentTypes)
            {
                switch (selectedComponentType)
                {
                    case WasteComponentType.Mercury:
                    case WasteComponentType.FGas:
                    case WasteComponentType.NORM:
                    case WasteComponentType.ODS:
                        wasteComponentInfos.Add(WasteComponentInfo.CreateWasteComponentInfo(selectedComponentType));
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unknown Waste Component Type {0}", selectedComponentType));
                }
            }
            return wasteComponentInfos;
        }

        public WasteComponentData Map(NotificationApplication source)
        {
            var wasteComponentData = new WasteComponentData();

            foreach (var wasteComponentInfo in source.WasteComponentInfos)
            {
                wasteComponentData.WasteComponentTypes.Add(wasteComponentInfo.WasteComponentType);
            }

            return wasteComponentData;
        }
    }
}
