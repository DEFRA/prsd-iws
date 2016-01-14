namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class FacilityCollection : Entity
    {
        protected FacilityCollection()
        {
        }

        public FacilityCollection(Guid notificationId)
        {
            NotificationId = notificationId;

            FacilitiesCollection = new List<Facility>();
        }

        public Guid NotificationId { get; private set; }

        public bool? AllFacilitiesPreconsented { get; internal set; }

        protected virtual ICollection<Facility> FacilitiesCollection { get; set; }

        public IEnumerable<Facility> Facilities
        {
            get { return FacilitiesCollection.ToSafeIEnumerable(); }
        }

        public bool IsInterim
        {
            get { return FacilitiesCollection != null && FacilitiesCollection.Skip(1).Any(); }
        }

        public Facility AddFacility(Business business, Address address, Contact contact)
        {
            var facility = new Facility(business, address, contact);

            FacilitiesCollection.Add(facility);
            return facility;
        }

        public Facility GetFacility(Guid facilityId)
        {
            var facility = FacilitiesCollection.SingleOrDefault(p => p.Id == facilityId);
            if (facility == null)
            {
                throw new InvalidOperationException(
                    string.Format("Facility with id {0} does not exist on this notification {1}", facilityId, NotificationId));
            }
            return facility;
        }

        public void RemoveFacility(Guid facilityId)
        {
            var facility = GetFacility(facilityId);
            if (facility.IsActualSiteOfTreatment && FacilitiesCollection.Count > 1)
            {
                throw new InvalidOperationException(string.Format("Cannot remove facility with id {0} for notification {1} without making another facility as actual site of treatment",
                   facilityId, NotificationId));
            }

            FacilitiesCollection.Remove(facility);
        }

        public void SetFacilityAsSiteOfTreatment(Guid facilityId)
        {
            if (FacilitiesCollection.All(p => p.Id != facilityId))
            {
                throw new InvalidOperationException(
                    string.Format("Unable to make facility with id {0} the site of treatment on this notification {1}", facilityId, NotificationId));
            }

            foreach (var facility in FacilitiesCollection)
            {
                facility.IsActualSiteOfTreatment = facility.Id == facilityId;
            }
        }
    }
}