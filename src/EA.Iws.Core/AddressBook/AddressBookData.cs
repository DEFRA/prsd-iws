namespace EA.Iws.Core.AddressBook
{
    using System;
    using System.Collections.Generic;

    public class AddressBookData
    {
        public int Count { get; set; }

        public Guid Id { get; set; }

        public List<AddressBookRecordData> AddressRecords { get; set; }
    }
}
