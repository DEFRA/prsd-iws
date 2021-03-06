﻿namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Carriers;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Notification = Domain.NotificationApplication.NotificationApplication;

    internal class CarrierDataMap : IMap<Notification, IList<CarrierData>>,
        IMap<Carrier, CarrierData>,
        IMapWithParentObjectId<Carrier, CarrierData>
    {
        private readonly ICarrierRepository repository;
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public CarrierDataMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessInfoData> businessMap,
            IMap<Contact, ContactData> contactMap,
            ICarrierRepository repository)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
            this.repository = repository;
        }

        public CarrierData Map(Carrier source)
        {
            return new CarrierData
            {
                Address = addressMap.Map(source.Address),
                Business = businessMap.Map(source.Business),
                Contact = contactMap.Map(source.Contact),
                Id = source.Id
            };
        }

        public IList<CarrierData> Map(Notification source)
        {
            var carrierCollection = Task.Run(() => repository.GetByNotificationId(source.Id)).Result;

            return carrierCollection.Carriers.Select(c => new CarrierData
            {
                Address = addressMap.Map(c.Address),
                Business = businessMap.Map(c.Business),
                Contact = contactMap.Map(c.Contact),
                Id = c.Id,
                NotificationId = source.Id
            }).ToList();
        }

        public CarrierData Map(Carrier source, Guid parentId)
        {
            var carrier = Map(source);
            carrier.NotificationId = parentId;
            return carrier;
        }
    }
}