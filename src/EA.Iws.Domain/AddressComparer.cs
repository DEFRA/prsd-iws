namespace EA.Iws.Domain
{
    using System.Collections.Generic;

    public class AddressComparer : IEqualityComparer<Address>
    {
        public bool Equals(Address x, Address y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(null, x))
            {
                return false;
            }

            if (ReferenceEquals(null, y))
            {
                return false;
            }

            return EqualsInternal(x, y);
        }

        public int GetHashCode(Address obj)
        {
            unchecked
            {
                var hashCode = 19;
                hashCode = (hashCode * 397) ^ (obj.Address1 != null ? obj.Address1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Address2 != null ? obj.Address2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.TownOrCity != null ? obj.TownOrCity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Region != null ? obj.Region.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.PostalCode != null ? obj.PostalCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Country != null ? obj.Country.GetHashCode() : 0);
                return hashCode;
            }
        }

        private bool EqualsInternal(Address x, Address y)
        {
            return string.Equals(x.Address1, y.Address1)
                   && string.Equals(x.Address2, y.Address2)
                   && string.Equals(x.TownOrCity, y.TownOrCity)
                   && string.Equals(x.Region, y.Region)
                   && string.Equals(x.PostalCode, y.PostalCode)
                   && string.Equals(x.Country, y.Country);
        }
    }
}