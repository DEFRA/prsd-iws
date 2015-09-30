namespace EA.Iws.Web.Mappings
{
    using Areas.NotificationApplication.ViewModels.Carrier;
    using Areas.NotificationApplication.ViewModels.Facility;
    using Areas.NotificationApplication.ViewModels.Producer;
    using Core.AddressBook;
    using Prsd.Core.Mapper;
    using Requests.AddressBook;

    public class AddAddressBookEntryMap : IMap<AddProducerViewModel, AddAddressBookEntry>,
        IMap<AddCarrierViewModel, AddAddressBookEntry>,
        IMap<AddFacilityViewModel, AddAddressBookEntry>
    {
        public AddAddressBookEntry Map(AddProducerViewModel source)
        {
            return new AddAddressBookEntry
            {
                Address = source.Address,
                Business = source.Business.ToBusinessInfoData(),
                Contact = source.Contact,
                Type = AddressRecordType.Producer
            };
        }

        public AddAddressBookEntry Map(AddCarrierViewModel source)
        {
            return new AddAddressBookEntry
            {
                Address = source.Address,
                Business = source.Business.ToBusinessInfoData(),
                Contact = source.Contact,
                Type = AddressRecordType.Carrier
            };
        }

        public AddAddressBookEntry Map(AddFacilityViewModel source)
        {
            return new AddAddressBookEntry
            {
                Address = source.Address,
                Business = source.Business.ToBusinessInfoData(),
                Contact = source.Contact,
                Type = AddressRecordType.Facility
            };
        }
    }
}