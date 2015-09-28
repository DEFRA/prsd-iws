namespace EA.Iws.Core.AddressBook
{
    using System;
    using Shared;

    public class AddressBookRecordData
    {
        public Guid Id { get; set; }

        public AddressData AddressData { get; set; }

        public ContactData ContactData { get; set; }

        public BusinessInfoData BusinessData { get; set; }
    }
}
