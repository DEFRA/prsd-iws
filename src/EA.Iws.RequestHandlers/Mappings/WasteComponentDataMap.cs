namespace EA.Iws.RequestHandlers.Mappings
{
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Prsd.Core.Mapper;
    using System.Collections.Generic;

    internal class WasteComponentDataMap : IMap<List<WasteComponentInfo>, WasteComponentData>
    {
        public WasteComponentData Map(List<WasteComponentInfo> source)
        {
            var data = new WasteComponentData();

            foreach (var item in source)
            {
                data.WasteComponentTypes.Add(item.WasteComponentType);
            }

            return data;
        }
    }
}
