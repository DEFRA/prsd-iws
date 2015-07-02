namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Producers;
    using Core.Shared;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Notification = Domain.Notification.NotificationApplication;

    internal class ProducerDataMap : IMap<Notification, IList<ProducerData>>, 
        IMap<Producer, ProducerData>,
        IMapWithParentObjectId<Producer, ProducerData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public ProducerDataMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessInfoData> businessMap,
            IMap<Contact, ContactData> contactMap)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
        }

        public ProducerData Map(Producer source)
        {
            return new ProducerData
            {
                Address = addressMap.Map(source.Address),
                Business = businessMap.Map(source.Business),
                Contact = contactMap.Map(source.Contact),
                Id = source.Id,
                IsSiteOfExport = source.IsSiteOfExport
            };
        }

        public IList<ProducerData> Map(Notification source)
        {
            return source.Producers.Select(p => new ProducerData
            {
                Address = addressMap.Map(p.Address),
                Business = businessMap.Map(p.Business),
                Contact = contactMap.Map(p.Contact),
                Id = p.Id,
                IsSiteOfExport = p.IsSiteOfExport,
                NotificationId = source.Id
            }).ToList();
        }

        public ProducerData Map(Producer source, Guid parentId)
        {
            var producer = Map(source);
            producer.NotificationId = parentId;
            return producer;
        }
    }
}
