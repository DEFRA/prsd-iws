namespace EA.Iws.RequestHandlers.Tests.Unit.Helpers
{
    using System.Collections.Generic;
    using Core.Shared;

    public class AddressDataEqualityComparer : IEqualityComparer<AddressData>
    {
        public bool Equals(AddressData x, AddressData y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.StreetOrSuburb == y.StreetOrSuburb
                   && x.Address2 == y.Address2
                   && x.CountryId == y.CountryId
                   && x.CountryName == y.CountryName
                   && x.PostalCode == y.PostalCode
                   && x.Region == y.Region;
        }

        public int GetHashCode(AddressData obj)
        {
            return base.GetHashCode();
        }
    }
}