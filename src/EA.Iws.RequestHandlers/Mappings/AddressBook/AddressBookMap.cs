namespace EA.Iws.RequestHandlers.Mappings.AddressBook
{
    using System.Linq;
    using Core.AddressBook;
    using Domain.AddressBook;
    using Prsd.Core.Mapper;

    internal class AddressBookMap : IMap<AddressBook, AddressBookData>
    {
        private readonly IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap;

        public AddressBookMap(IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap)
        {
            this.addressBookRecordMap = addressBookRecordMap;
        }

        public AddressBookData Map(AddressBook source)
        {
            return new AddressBookData
            {
                Id = source.Id,
                Type = source.Type,
                AddressRecords = source.Addresses.Select(addressBookRecordMap.Map).ToList()
            };
        }
    }
}
