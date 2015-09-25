namespace EA.Iws.Web.Mappings
{
    using Areas.NotificationApplication.ViewModels.Producer;
    using Core.AddressBook;
    using Prsd.Core.Mapper;
    using Requests.AddressBook;

    public class AddAddressBookEntryMap : IMap<AddProducerViewModel, AddAddressBookEntry>
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
    }
}