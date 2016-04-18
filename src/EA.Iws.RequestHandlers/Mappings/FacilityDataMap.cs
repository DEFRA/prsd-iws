namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Facilities;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Notification = Domain.NotificationApplication.NotificationApplication;

    internal class FacilityDataMap : IMap<Notification, IList<FacilityData>>,
        IMap<Facility, FacilityData>,
        IMapWithParentObjectId<Facility, FacilityData>
    {
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;
        private readonly IFacilityRepository facilityRepository;

        public FacilityDataMap(IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessInfoData> businessMap,
            IMap<Contact, ContactData> contactMap,
            IFacilityRepository facilityRepository)
        {
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
            this.facilityRepository = facilityRepository;
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
            var facilityCollection = Task.Run(() => facilityRepository.GetByNotificationId(source.Id)).Result;

            return facilityCollection.Facilities.Select(f => new FacilityData
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