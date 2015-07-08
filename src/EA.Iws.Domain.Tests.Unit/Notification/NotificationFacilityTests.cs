namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationFacilityTests
    {
        private readonly NotificationApplication notification;
        private readonly Facility facility1;
        private readonly Facility facility2;
        private static readonly Guid FacilityId = new Guid("F4F79CE8-2A52-4351-9A71-EC31A6C13D9D");

        public NotificationFacilityTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                           UKCompetentAuthority.England, 0);

            facility1 = notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                        ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());
            facility2 = notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                        ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());
        }

        [Fact]
        public void FacilitiesCanOnlyHaveOneSiteOfTreatment()
        {
            EntityHelper.SetEntityIds(facility1, facility2);
            notification.SetFacilityAsSiteOfTreatment(facility1.Id);
            var siteOfTreatmentCount = notification.Facilities.Count(p => p.IsActualSiteOfTreatment);
            Assert.Equal(1, siteOfTreatmentCount);
        }

        [Fact]
        public void CantSetNonExistentFacilityAsSiteOfTreatment()
        {
            EntityHelper.SetEntityId(facility1, FacilityId);
            var badId = new Guid("{5DF206F6-4116-4EEC-949A-0FC71FE609C1}");
            Action setAsSiteOfTreatment = () => notification.SetFacilityAsSiteOfTreatment(badId);
            Assert.Throws<InvalidOperationException>(setAsSiteOfTreatment);
        }

        [Fact]
        public void CantRemoveNonExistentFacility()
        {
            Action removeFacility = () => notification.RemoveFacility(FacilityId);
            Assert.Throws<InvalidOperationException>(removeFacility);
        }

        [Fact]
        public void RemoveExistingFacilityByFacilityId()
        {
            Facility tempFacility = notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempFacility, FacilityId);

            int beforeFacilitiesCount = notification.Facilities.Count();
            notification.RemoveFacility(FacilityId);
            int afterFacilitiesCount = notification.Facilities.Count();

            Assert.True(afterFacilitiesCount == beforeFacilitiesCount - 1);
        }

        [Fact]
        public void CanRemoveFacilityOtherThanActualSiteOfTreatment()
        {
            Facility tempFacility = notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempFacility, FacilityId);

            int beforeFacilitiesCount = notification.Facilities.Count();
            notification.RemoveFacility(FacilityId);
            Assert.True(notification.Facilities.Count() == beforeFacilitiesCount - 1);
        }

        [Fact]
        public void CannotRemoveExistingActualSiteOfTreatmentFacilityByFacilityIdForMoreThanOneFacilities()
        {
            Facility tempFacility = notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempFacility, FacilityId);
            notification.SetFacilityAsSiteOfTreatment(FacilityId);
            notification.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                                     ObjectFactory.CreateDefaultAddress(),
                                     ObjectFactory.CreateEmptyContact());
            Assert.True(notification.Facilities.Count() == 4);
            Assert.True(notification.Facilities.Count(x => x.IsActualSiteOfTreatment) == 1);

            Action removeActualSiteOfTreatmentFacility = (() => notification.RemoveFacility(FacilityId));
            Assert.Throws<InvalidOperationException>(removeActualSiteOfTreatmentFacility);
        }
    }
}