namespace EA.Iws.Domain
{
    using Prsd.Core.Domain;

    public class BusinessType : Enumeration
    {
        public static readonly BusinessType LimitedCompany = new BusinessType(1, "Limited Company");
        public static readonly BusinessType SoleTrader = new BusinessType(2, "Sole Trader");
        public static readonly BusinessType Partnership = new BusinessType(3, "Partnership");
        public static readonly BusinessType Other = new BusinessType(4, "Other");

        protected BusinessType()
        {
        }

        private BusinessType(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}