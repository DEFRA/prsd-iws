namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Carriers;
    using Core.Shared;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.Carriers;
    using Requests.Shared;
    using Notification = Domain.Notification.NotificationApplication;

    internal class CarrierDataMap : IMap<Notification, IList<CarrierData>>,
        IMap<Carrier, CarrierData>,
        IMapWithParentObjectId<Carrier, CarrierData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public CarrierDataMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessData> businessMap,
            IMap<Contact, ContactData> contactMap)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
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
            return source.Carriers.Select(c => new CarrierData
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
