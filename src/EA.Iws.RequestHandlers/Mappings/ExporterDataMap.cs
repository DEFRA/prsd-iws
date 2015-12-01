namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Core.Exporters;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.Exporter;
    using Prsd.Core.Mapper;

    internal class ExporterDataMap : IMap<Exporter, ExporterData>,
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
            if (source == null)
            {
                return new ExporterData();
            }

            return new ExporterData
            {
                Address = addressMap.Map(source.Address),
                Business = businessMap.Map(source.Business),
                Contact = contactMap.Map(source.Contact),
                Id = source.Id,
                NotificationId = source.NotificationId,
                HasExporter = true
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