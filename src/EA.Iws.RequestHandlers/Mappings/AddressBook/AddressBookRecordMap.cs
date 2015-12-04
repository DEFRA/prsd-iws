namespace EA.Iws.RequestHandlers.Mappings.AddressBook
{
    using Core.AddressBook;
    using Core.Shared;
    using Domain;
    using Domain.AddressBook;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class AddressBookRecordMap : IMap<AddressBookRecord, AddressBookRecordData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public AddressBookRecordMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessInfoData> businessMap,
            IMap<Contact, ContactData> contactMap)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
        }

        public AddressBookRecordData Map(AddressBookRecord source)
        {
            return new AddressBookRecordData
            {
                Id = source.Id,
                AddressData = addressMap.Map(source.Address),
                BusinessData = businessMap.Map(source.Business),
                ContactData = contactMap.Map(source.Contact)
            };
        }
    }
}
