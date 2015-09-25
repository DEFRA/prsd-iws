namespace EA.Iws.Domain.AddressBook
{
    using System.Collections.Generic;

    public class AddressBookRecordComparer : IEqualityComparer<AddressBookRecord>
    {
        private static readonly AddressComparer AddressComparer = new AddressComparer();
        private static readonly BusinessComparer BusinessComparer = new BusinessComparer();
        private static readonly ContactComparer ContactComparer = new ContactComparer();

        public bool Equals(AddressBookRecord x, AddressBookRecord y)
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

            return AddressComparer.Equals(x.Address, y.Address)
                   && BusinessComparer.Equals(x.Business, y.Business)
                   && ContactComparer.Equals(x.Contact, y.Contact);
        }

        public int GetHashCode(AddressBookRecord obj)
        {
            unchecked
            {
                var hashCode = 19;
                hashCode = (hashCode * 397) ^ (obj.Address != null ? AddressComparer.GetHashCode(obj.Address) : 0);
                hashCode = (hashCode * 397) ^ (obj.Business != null ? BusinessComparer.GetHashCode(obj.Business) : 0);
                hashCode = (hashCode * 397) ^ (obj.Contact != null ? ContactComparer.GetHashCode(obj.Contact) : 0);
                return hashCode;
            }
        }
    }
}
