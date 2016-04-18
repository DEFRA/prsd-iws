namespace EA.Iws.RequestHandlers.Tests.Unit.Helpers
{
    using System.Collections.Generic;
    using Core.Shared;

    public class BusinessInfoDataEqualityComparer : IEqualityComparer<BusinessInfoData>
    {
        public int GetHashCode(BusinessInfoData obj)
        {
            return base.GetHashCode();
        }

        public bool Equals(BusinessInfoData x, BusinessInfoData y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.AdditionalRegistrationNumber == y.AdditionalRegistrationNumber
                   && x.BusinessType == y.BusinessType
                   && x.Name == y.Name
                   && x.OtherDescription == y.OtherDescription
                   && x.RegistrationNumber == y.RegistrationNumber;
        }
    }
}