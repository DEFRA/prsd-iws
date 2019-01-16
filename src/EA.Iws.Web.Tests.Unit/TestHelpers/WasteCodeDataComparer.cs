namespace EA.Iws.Web.Tests.Unit.TestHelpers
{
    using System.Collections.Generic;
    using Core.WasteCodes;

    public class WasteCodeDataComparer : IEqualityComparer<WasteCodeData>
    {
        public bool Equals(WasteCodeData x, WasteCodeData y)
        {
            return x != null && y != null && x.Code == y.Code;
        }

        public int GetHashCode(WasteCodeData obj)
        {
            return obj.Code.GetHashCode();
        }
    }
}
