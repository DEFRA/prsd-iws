namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;

    public class ContactComparer : IEqualityComparer<Contact>
    {
        public bool Equals(Contact x, Contact y)
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

        public int GetHashCode(Contact obj)
        {
            unchecked
            {
                var hashCode = 19;
                hashCode = (hashCode * 397) ^ (obj.Email != null ? obj.Email.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Fax != null ? obj.Fax.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.FullName != null ? obj.FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Telephone != null ? obj.Telephone.GetHashCode() : 0);
                return hashCode;
            }
        }

        private bool EqualsInternal(Contact x, Contact y)
        {
            return string.Equals(x.Email, y.Email)
                   && string.Equals(x.Fax, y.Fax)
                   && string.Equals(x.FullName, y.FullName)
                   && string.Equals(x.Telephone, y.Telephone);
        }
    }
}