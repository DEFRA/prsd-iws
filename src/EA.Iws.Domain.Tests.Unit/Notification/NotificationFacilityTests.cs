namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using Helpers;
    using Xunit;

    public class NotificationFacilityTests
    {
        [Fact]
        public void FacilitiesCanOnlyHaveOneSiteOfTreatment()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var facility1 = notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());
            var facility2 = notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

            EntityHelper.SetEntityIds(facility1, facility2);

            notification.SetFacilityAsSiteOfTreatment(facility1.Id);

            var siteOfTreatmentCount = notification.Facilities.Count(p => p.IsActualSiteOfTreatment);
            Assert.Equal(1, siteOfTreatmentCount);
        }

        [Fact]
        public void CantSetNonExistentFacilityAsSiteOfTreatment()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var facility = notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(facility, new Guid("{D65D91BA-FA77-47F6-ACF5-B1A405DEE187}"));

            var badId = new Guid("{5DF206F6-4116-4EEC-949A-0FC71FE609C1}");

            Action setAsSiteOfTreatment = () => notification.SetFacilityAsSiteOfTreatment(badId);

            Assert.Throws<InvalidOperationException>(setAsSiteOfTreatment);
        }

        [Fact]
        public void CantRemoveNonExistentFacility()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action removeFacility =
                () => notification.RemoveFacility(new Guid("{BD49EF90-C9B2-4E84-B0D3-964BC2A592D5}"));

            Assert.Throws<InvalidOperationException>(removeFacility);
        }
    }
}