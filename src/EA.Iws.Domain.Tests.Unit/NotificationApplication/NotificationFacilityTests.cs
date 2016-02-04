namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Linq;
    using Core.Shared;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class NotificationFacilityTests
    {
        private readonly NotificationApplication notification;
        private readonly FacilityCollection facilityCollection;
        private readonly Facility facility1;
        private readonly Facility facility2;
        private static readonly Guid FacilityId = new Guid("F4F79CE8-2A52-4351-9A71-EC31A6C13D9D");

        public NotificationFacilityTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                           CompetentAuthorityEnum.England, 0);

            facilityCollection = new FacilityCollection(notification.Id);

            facility1 = facilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                        ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());
            facility2 = facilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                        ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());
        }

        [Fact]
        public void FacilitiesCanOnlyHaveOneSiteOfTreatment()
        {
            EntityHelper.SetEntityIds(facility1, facility2);
            facilityCollection.SetFacilityAsSiteOfTreatment(facility1.Id);
            var siteOfTreatmentCount = facilityCollection.Facilities.Count(p => p.IsActualSiteOfTreatment);
            Assert.Equal(1, siteOfTreatmentCount);
        }

        [Fact]
        public void CantSetNonExistentFacilityAsSiteOfTreatment()
        {
            EntityHelper.SetEntityId(facility1, FacilityId);
            var badId = new Guid("{5DF206F6-4116-4EEC-949A-0FC71FE609C1}");
            Action setAsSiteOfTreatment = () => facilityCollection.SetFacilityAsSiteOfTreatment(badId);
            Assert.Throws<InvalidOperationException>(setAsSiteOfTreatment);
        }

        [Fact]
        public void CantRemoveNonExistentFacility()
        {
            Action removeFacility = () => facilityCollection.RemoveFacility(FacilityId);
            Assert.Throws<InvalidOperationException>(removeFacility);
        }

        [Fact]
        public void RemoveExistingFacilityByFacilityId()
        {
            Facility tempFacility = facilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempFacility, FacilityId);

            int beforeFacilitiesCount = facilityCollection.Facilities.Count();
            facilityCollection.RemoveFacility(FacilityId);
            int afterFacilitiesCount = facilityCollection.Facilities.Count();

            Assert.True(afterFacilitiesCount == beforeFacilitiesCount - 1);
        }

        [Fact]
        public void CanRemoveFacilityOtherThanActualSiteOfTreatment()
        {
            Facility tempFacility = facilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempFacility, FacilityId);

            int beforeFacilitiesCount = facilityCollection.Facilities.Count();
            facilityCollection.RemoveFacility(FacilityId);
            Assert.True(facilityCollection.Facilities.Count() == beforeFacilitiesCount - 1);
        }

        [Fact]
        public void CannotRemoveExistingActualSiteOfTreatmentFacilityByFacilityIdForMoreThanOneFacilities()
        {
            Facility tempFacility = facilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempFacility, FacilityId);
            facilityCollection.SetFacilityAsSiteOfTreatment(FacilityId);
            facilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(),
                                     ObjectFactory.CreateDefaultAddress(),
                                     ObjectFactory.CreateEmptyContact());
            Assert.True(facilityCollection.Facilities.Count() == 4);
            Assert.True(facilityCollection.Facilities.Count(x => x.IsActualSiteOfTreatment) == 1);

            Action removeActualSiteOfTreatmentFacility = (() => facilityCollection.RemoveFacility(FacilityId));
            Assert.Throws<InvalidOperationException>(removeActualSiteOfTreatmentFacility);
        }

        [Fact]
        public void HasMultipleFacilities_MultipleFacilities_IsTrue()
        {
            facilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(),
                ObjectFactory.CreateEmptyContact());

            facilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(),
                ObjectFactory.CreateEmptyContact());

            Assert.True(facilityCollection.HasMultipleFacilities);
        }

        [Fact]
        public void HasMultipleFacilities_SingleFacility_IsFalse()
        {
            var newFacilityCollection = new FacilityCollection(notification.Id);
            newFacilityCollection.AddFacility(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(),
                ObjectFactory.CreateEmptyContact());

            Assert.False(newFacilityCollection.HasMultipleFacilities);
        }

        [Fact]
        public void HasMultipleFacilitiesFalseByDefault()
        {
            var newFacilityCollection = new FacilityCollection(notification.Id);

            Assert.False(newFacilityCollection.HasMultipleFacilities);
        }
    }
}