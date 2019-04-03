namespace EA.Iws.Web.Tests.Unit.TestHelpers
{
    using System.Collections.Generic;
    using Core.ImportNotification.Summary;

    public class WasteCodeComparer : IEqualityComparer<WasteCode>
    {
        public bool Equals(WasteCode x, WasteCode y)
        {
            return y != null && x != null && x.Name == y.Name;
        }

        public int GetHashCode(WasteCode obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
