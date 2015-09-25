namespace EA.Iws.Requests.AddressBook
{
    using System.Collections.Generic;
    using Core.AddressBook;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    public class SearchAddressRecords : IRequest<IList<AddressBookRecordData>>
    {
        public string SearchTerm { get; private set; }

        public AddressRecordType Type { get; private set; }

        public SearchAddressRecords(string searchTerm, AddressRecordType type)
        {
            Guard.ArgumentNotNull(() => searchTerm, searchTerm);
            SearchTerm = searchTerm;
            Type = type;
        }
    }
}
