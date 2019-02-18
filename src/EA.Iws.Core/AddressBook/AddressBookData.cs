namespace EA.Iws.Core.AddressBook
{
    using System;
    using System.Collections.Generic;

    public class AddressBookData
    {
        public Guid Id { get; set; }

        public List<AddressBookRecordData> AddressRecords { get; set; }

        public AddressRecordType Type { get; set; }

        public string SearchTerm { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int NumberOfMatchedRecords { get; set; }

        public bool IsInternalUser { get; set; }

        public AddressBookData()
        {
            AddressRecords = new List<AddressBookRecordData>();
        }
    }
}
