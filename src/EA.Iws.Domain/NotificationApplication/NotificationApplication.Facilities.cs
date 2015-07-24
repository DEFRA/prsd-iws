namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Linq;

    public partial class NotificationApplication
    {
        public bool? IsPreconsentedRecoveryFacility { get; private set; }

        public Facility AddFacility(Business business, Address address, Contact contact)
        {
            var facility = new Facility(business, address, contact);
            if (!FacilitiesCollection.Any())
            {
                facility.IsActualSiteOfTreatment = true;
            }

            FacilitiesCollection.Add(facility);
            return facility;
        }

        public Facility GetFacility(Guid facilityId)
        {
            var facility = FacilitiesCollection.SingleOrDefault(p => p.Id == facilityId);
            if (facility == null)
            {
                throw new InvalidOperationException(
                    string.Format("Facility with id {0} does not exist on this notification {1}", facilityId, Id));
            }
            return facility;
        }

        public void RemoveFacility(Guid facilityId)
        {
            var facility = GetFacility(facilityId);
            if (facility.IsActualSiteOfTreatment && FacilitiesCollection.Count > 1)
            {
                throw new InvalidOperationException(string.Format("Cannot remove facility with id {0} for notification {1} without making another facility as actual site of treatment",
                   facilityId, Id));
            }

            FacilitiesCollection.Remove(facility);
        }

        public void SetFacilityAsSiteOfTreatment(Guid facilityId)
        {
            if (FacilitiesCollection.All(p => p.Id != facilityId))
            {
                throw new InvalidOperationException(
                    string.Format("Unable to make facility with id {0} the site of treatment on this notification {1}", facilityId, Id));
            }

            foreach (var facility in FacilitiesCollection)
            {
                facility.IsActualSiteOfTreatment = facility.Id == facilityId;
            }
        }

        public void SetPreconsentedRecoveryFacility(bool isPreconsentedRecoveryFacility)
        {
            if (NotificationType != NotificationType.Recovery)
            {
                throw new InvalidOperationException(String.Format(
                    "Can't set pre-consented recovery facility as notification type is not Recovery for notification: {0}", Id));
            }

            IsPreconsentedRecoveryFacility = isPreconsentedRecoveryFacility;
        }
    }
}