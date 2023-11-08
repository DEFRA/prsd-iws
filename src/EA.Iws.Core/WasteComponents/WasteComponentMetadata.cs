namespace EA.Iws.Core.WasteComponentType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WasteComponentMetadata
    {
        public static IEnumerable<WasteComponentType> GetWasteComponents()
        {
            return Enum.GetValues(typeof(WasteComponentType)).Cast<WasteComponentType>();
        }
    }
}
