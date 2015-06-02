namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.Exporters;
    using Requests.Shared;
    using Notification = Domain.Notification.NotificationApplication;

    internal class ExporterDataMap : IMap<Notification, ExporterData>, 
        IMap<Exporter, ExporterData>,
        IMapWithParentObjectId<Exporter, ExporterData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public ExporterDataMap(IMap<Address, AddressData> addressMap, 
            IMap<Business, BusinessData> businessMap,
            IMap<Contact, ContactData> contactMap)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
        }

        public ExporterData Map(Exporter source)
        {
            return new ExporterData
            {
                Address = addressMap.Map(source.Address),
                Business = businessMap.Map(source.Business),
                Contact = contactMap.Map(source.Contact),
                Id = source.Id
            };
        }

        public ExporterData Map(NotificationApplication source)
        {
            return new ExporterData
            {
                Address = addressMap.Map(source.Exporter.Address),
                Business = businessMap.Map(source.Exporter.Business),
                Contact = contactMap.Map(source.Exporter.Contact),
                Id = source.Exporter.Id,
                NotificationId = source.Id
            };
        }

        public ExporterData Map(Exporter source, Guid parentId)
        {
            var exporter = Map(source);
            exporter.NotificationId = parentId;
            return exporter;
        }
    }
}
