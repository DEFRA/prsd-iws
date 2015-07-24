namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Core.Exporters;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Exporters;
    using Requests.Shared;
    using Notification = Domain.NotificationApplication.NotificationApplication;

    internal class ExporterDataMap : IMap<Notification, ExporterData>, 
        IMap<Exporter, ExporterData>,
        IMapWithParentObjectId<Exporter, ExporterData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public ExporterDataMap(IMap<Address, AddressData> addressMap, 
            IMap<Business, BusinessInfoData> businessMap,
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
                Id = source.Id,
                HasExporter = true
            };
        }

        public ExporterData Map(NotificationApplication source)
        {
            if (source.HasExporter)
            {
                return new ExporterData
                {
                    Address = addressMap.Map(source.Exporter.Address),
                    Business = businessMap.Map(source.Exporter.Business),
                    Contact = contactMap.Map(source.Exporter.Contact),
                    Id = source.Exporter.Id,
                    NotificationId = source.Id,
                    HasExporter = true
                };
            }
            else
            {
                return new ExporterData
                {
                    NotificationId = source.Id,
                    HasExporter = false
                };
            }
        }

        public ExporterData Map(Exporter source, Guid parentId)
        {
            var exporter = Map(source);
            exporter.NotificationId = parentId;
            return exporter;
        }
    }
}