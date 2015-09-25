namespace EA.Iws.Domain.AddressBook
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.AddressBook;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class AddressBook : Entity
    {
        private static readonly AddressBookRecordComparer AddressBookRecordComparer 
            = new AddressBookRecordComparer();

        protected AddressBook()
        {
        }

        public AddressBook(IEnumerable<AddressBookRecord> addresses,
            AddressRecordType type,
            Guid userId)
        {
            UserId = userId;
            AddressCollection = addresses.ToList();
            Type = type;
        }

        protected virtual ICollection<AddressBookRecord> AddressCollection { get; set; }

        public Guid UserId { get; set; }

        public IEnumerable<AddressBookRecord> Addresses
        {
            get { return AddressCollection.ToSafeIEnumerable(); }
        }

        public AddressRecordType Type { get; private set; }

        public void AddAddress(AddressBookRecord addressBookRecord)
        {
            if (!AddressCollection.Contains(addressBookRecord, AddressBookRecordComparer))
            {
                AddressCollection.Add(addressBookRecord);
            }
        }
    }
}