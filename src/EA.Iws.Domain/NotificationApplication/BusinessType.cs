namespace EA.Iws.Domain.NotificationApplication
{
    using System;
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

        public static BusinessType FromBusinessType(Core.Shared.BusinessType businessType)
        {
            switch (businessType)
            {
                case Core.Shared.BusinessType.Other:
                    return Other;
                case Core.Shared.BusinessType.Partnership:
                    return Partnership;
                case Core.Shared.BusinessType.SoleTrader:
                    return SoleTrader;
                case Core.Shared.BusinessType.LimitedCompany:
                    return LimitedCompany;
                default:
                    throw new ArgumentException(string.Format("Unknown business type: {0}", businessType), "businessType");
            }
        }

        public static explicit operator Core.Shared.BusinessType(BusinessType businessType)
        {
            if (businessType == LimitedCompany)
            {
                return Core.Shared.BusinessType.LimitedCompany;
            }
            if (businessType == Partnership)
            {
                return Core.Shared.BusinessType.Partnership;
            }
            if (businessType == SoleTrader)
            {
                return Core.Shared.BusinessType.SoleTrader;
            }
            return Core.Shared.BusinessType.Other;
        }
    }
}