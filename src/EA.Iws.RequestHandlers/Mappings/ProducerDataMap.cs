namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Producers;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Notification = Domain.NotificationApplication.NotificationApplication;

    internal class ProducerDataMap : IMap<Notification, IList<ProducerData>>,
        IMap<Producer, ProducerData>,
        IMapWithParentObjectId<Producer, ProducerData>
    {
        private readonly IProducerRepository producerRepository;
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public ProducerDataMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessInfoData> businessMap,
            IMap<Contact, ContactData> contactMap,
            IProducerRepository producerRepository)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
            this.producerRepository = producerRepository;
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
            var producerCollection = Task.Run(() => producerRepository.GetByNotificationId(source.Id)).Result;
            return producerCollection.Producers.Select(p => new ProducerData
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
