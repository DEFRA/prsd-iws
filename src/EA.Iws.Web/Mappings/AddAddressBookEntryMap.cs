namespace EA.Iws.Web.Mappings
{
    using System;
    using Areas.NotificationApplication.ViewModels.Carrier;
    using Areas.NotificationApplication.ViewModels.Exporter;
    using Areas.NotificationApplication.ViewModels.Facility;
    using Areas.NotificationApplication.ViewModels.Producer;
    using Core.AddressBook;
    using Core.Shared;
    using Prsd.Core.Mapper;
    using Requests.AddressBook;

    public class AddAddressBookEntryMap : IMap<AddProducerViewModel, AddAddressBookEntry>,
        IMap<AddCarrierViewModel, AddAddressBookEntry>,
        IMap<AddFacilityViewModel, AddAddressBookEntry>,
        IMapWithParameter<ExporterViewModel, AddressRecordType, AddAddressBookEntry>
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

        public AddAddressBookEntry Map(ExporterViewModel source, AddressRecordType parameter)
        {
            switch (parameter)
            {
                 case AddressRecordType.Producer:
                    return new AddAddressBookEntry
                    {
                        Address = source.Address,
                        Business = RemoveRegistrationNumberDataForProducer(source),
                        Contact = source.Contact,
                        Type = AddressRecordType.Producer
                    };
                default:
                    throw new InvalidOperationException();
            }
        }

        private BusinessInfoData RemoveRegistrationNumberDataForProducer(ExporterViewModel model)
        {
            var businessData  = model.Business.ToBusinessInfoData();

            if (businessData.BusinessType != BusinessType.LimitedCompany)
            {
                businessData.RegistrationNumber = null;
            }

            businessData.AdditionalRegistrationNumber = null;

            return businessData;
        }
    }
}