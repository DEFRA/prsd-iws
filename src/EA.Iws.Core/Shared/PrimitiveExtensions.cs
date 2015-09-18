namespace EA.Iws.Core.Shared
{
    using System;

    public static class PrimitiveExtensions
    {
        public static bool IsDecimalValidToNDecimalPlaces(this decimal d, int n)
        {
            return Decimal.Round(d, n) == d;
        }
    }
}
