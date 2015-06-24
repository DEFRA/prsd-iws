namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.Importer;
    using Requests.Shared;
    using Notification = Domain.Notification.NotificationApplication;

    internal class ImporterDataMap : IMap<Notification, ImporterData>, 
        IMapWithParentObjectId<Importer, ImporterData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public ImporterDataMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessData> businessMap,
            IMap<Contact, ContactData> contactMap)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
        }

        public ImporterData Map(Notification source)
        {
            if (source.HasImporter)
            {
                return new ImporterData
                {
                    Address = addressMap.Map(source.Importer.Address),
                    Business = businessMap.Map(source.Importer.Business),
                    Contact = contactMap.Map(source.Importer.Contact),
                    Id = source.Importer.Id,
                    NotificationId = source.Id,
                    HasImporter = true
                };
            }
            else
            {
                return new ImporterData
                {
                    NotificationId = source.Id,
                    HasImporter = true
                };
            }
        }

        public ImporterData Map(Importer source, Guid parentId)
        {
            return new ImporterData
            {
                Address = addressMap.Map(source.Address),
                Business = businessMap.Map(source.Business),
                Contact = contactMap.Map(source.Contact),
                Id = source.Id,
                NotificationId = parentId,
                HasImporter = true
            };
        }
    }
}
