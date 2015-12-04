namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;

    public class BusinessComparer : IEqualityComparer<Business>
    {
        public bool Equals(Business x, Business y)
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

        public int GetHashCode(Business obj)
        {
            unchecked
            {
                var hashCode = 19;
                hashCode = (hashCode * 397) ^
                           (obj.AdditionalRegistrationNumber != null
                               ? obj.AdditionalRegistrationNumber.GetHashCode()
                               : 0);
                hashCode = (hashCode * 397) ^ (obj.Name != null ? obj.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.OtherDescription != null ? obj.OtherDescription.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.RegistrationNumber != null ? obj.RegistrationNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Type != null ? obj.Type.GetHashCode() : 0);
                return hashCode;
            }
        }

        private bool EqualsInternal(Business x, Business y)
        {
            if (x.Type != y.Type)
            {
                return false;
            }

            return string.Equals(x.Name, y.Name)
                   && string.Equals(x.AdditionalRegistrationNumber, y.AdditionalRegistrationNumber)
                   && string.Equals(x.OtherDescription, y.OtherDescription)
                   && string.Equals(x.RegistrationNumber, y.RegistrationNumber);
        }
    }
}