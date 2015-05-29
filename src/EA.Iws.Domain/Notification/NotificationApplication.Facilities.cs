namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Linq;

    public partial class NotificationApplication
    {
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
                    string.Format("Facility with id {0} does not exist on this notification", facilityId));
            }
            return facility;
        }

        public void RemoveFacility(Guid facilityId)
        {
            var facility = GetFacility(facilityId);

            FacilitiesCollection.Remove(facility);
        }

        public void SetFacilityAsSiteOfTreatment(Guid facilityId)
        {
            if (FacilitiesCollection.All(p => p.Id != facilityId))
            {
                throw new InvalidOperationException(
                    string.Format("Unable to make facility with id {0} the site of treatment", facilityId));
            }

            foreach (var facility in FacilitiesCollection)
            {
                facility.IsActualSiteOfTreatment = facility.Id == facilityId;
            }
        }
    }
}