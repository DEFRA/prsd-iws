namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Importer;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Importer;
    using Prsd.Core.Mapper;

    internal class ImporterDataMap : IMap<Importer, ImporterData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public ImporterDataMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessInfoData> businessMap,
            IMap<Contact, ContactData> contactMap)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
        }

        public ImporterData Map(Importer source)
        {
            if (source == null)
            {
                return new ImporterData();
            }

            return new ImporterData
            {
                Address = addressMap.Map(source.Address),
                Business = businessMap.Map(source.Business),
                Contact = contactMap.Map(source.Contact),
                Id = source.Id,
                NotificationId = source.NotificationId,
                HasImporter = true
            };
        }
    }
}