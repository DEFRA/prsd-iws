namespace EA.Iws.Core.WasteComponentType
{
    using System.Collections.Generic;

    public class WasteComponentData
    {
        public WasteComponentData()
        {
            WasteComponentTypes = new List<WasteComponentType>();
        }

        public IList<WasteComponentType> WasteComponentTypes { get; set; }
    }
}
