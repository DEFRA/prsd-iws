namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.AddressBook;
    using Domain.AddressBook;
    using Helpers;

    public class TestableAddressBook : AddressBook
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<AddressBook>.SetProperty(x => x.Id, value, this); }
        }

        public new Guid UserId
        {
            get { return base.UserId; }
            set { ObjectInstantiator<AddressBook>.SetProperty(x => x.UserId, value, this); }
        }

        public new List<AddressBookRecord> Addresses
        {
            get { return AddressCollection.ToList(); }
            set { AddressCollection = value; }
        }

        public new AddressRecordType Type
        {
            get { return base.Type; }
            set { ObjectInstantiator<AddressBook>.SetProperty(x => x.Type, value, this); }
        }
    }
}
