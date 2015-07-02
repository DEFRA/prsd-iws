namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Facilities;
    using Core.Shared;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Notification = Domain.Notification.NotificationApplication;

    internal class FacilityDataMap : IMap<Notification, IList<FacilityData>>,
        IMap<Facility, FacilityData>,
        IMapWithParentObjectId<Facility, FacilityData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public FacilityDataMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessInfoData> businessMap,
            IMap<Contact, ContactData> contactMap)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
        }

        public FacilityData Map(Facility source)
        {
            return new FacilityData
            {
                Address = addressMap.Map(source.Address),
                Business = businessMap.Map(source.Business),
                Contact = contactMap.Map(source.Contact),
                Id = source.Id,
                IsActualSiteOfTreatment = source.IsActualSiteOfTreatment
            };
        }

        public IList<FacilityData> Map(Notification source)
        {
            return source.Facilities.Select(f => new FacilityData
            {
                Address = addressMap.Map(f.Address),
                Business = businessMap.Map(f.Business),
                Contact = contactMap.Map(f.Contact),
                Id = f.Id,
                NotificationId = source.Id,
                IsActualSiteOfTreatment = f.IsActualSiteOfTreatment
            }).ToList();
        }

        public FacilityData Map(Facility source, Guid parentId)
        {
            var facility = Map(source);
            facility.NotificationId = parentId;
            return facility;
        }
    }
}
