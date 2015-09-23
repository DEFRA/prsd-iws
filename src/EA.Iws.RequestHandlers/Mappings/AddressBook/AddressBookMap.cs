namespace EA.Iws.RequestHandlers.Mappings.AddressBook
{
    using System.Linq;
    using Core.AddressBook;
    using Core.Shared;
    using Domain;
    using Domain.AddressBook;
    using Prsd.Core.Mapper;

    internal class AddressBookMap : IMap<AddressBook, AddressBookData>
    {
        private readonly IMap<Address, AddressData> addressMap;

        public AddressBookMap(IMap<Address, AddressData> addressMap)
        {
            this.addressMap = addressMap;
        }

        public AddressBookData Map(AddressBook source)
        {
            return new AddressBookData
            {
                Id = source.Id,
                Count = source.Addresses.Count(),
                AddressRecords = source.Addresses.Select(a => new AddressBookRecordData
                {
                    AddressData = addressMap.Map(a.Address)
                }).ToList()
            };
        }
    }
}
