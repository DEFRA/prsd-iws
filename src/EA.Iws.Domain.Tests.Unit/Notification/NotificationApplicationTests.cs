namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using Helpers;
    using Xunit;

    public class NotificationApplicationTests
    {
        private const string England = "England";
        private const string Scotland = "Scotland";
        private const string NorthernIreland = "Northern Ireland";
        private const string Wales = "Wales";

        [Theory]
        [InlineData("GB 0001 123456", England, 123456)]
        [InlineData("GB 0002 123456", Scotland, 123456)]
        [InlineData("GB 0003 123456", NorthernIreland, 123456)]
        [InlineData("GB 0004 123456", Wales, 123456)]
        [InlineData("GB 0001 005000", England, 5000)]
        [InlineData("GB 0002 000100", Scotland, 100)]
        public void NotificationNumberFormat(string expected, string country, int notificationNumber)
        {
            var userId = new Guid("{FCCC2E8A-2464-4C10-8521-09F16F2C550C}");
            var notification = new NotificationApplication(userId, NotificationType.Disposal,
                GetCompetentAuthority(country),
                notificationNumber);
            Assert.Equal(expected, notification.NotificationNumber);
        }

        private static UKCompetentAuthority GetCompetentAuthority(string country)
        {
            if (country == England)
            {
                return UKCompetentAuthority.England;
            }
            if (country == Scotland)
            {
                return UKCompetentAuthority.Scotland;
            }
            if (country == NorthernIreland)
            {
                return UKCompetentAuthority.NorthernIreland;
            }
            if (country == Wales)
            {
                return UKCompetentAuthority.Wales;
            }
            throw new ArgumentException("Unknown competent authority", "country");
        }

        private static Contact CreateEmptyContact()
        {
            return new Contact(string.Empty, String.Empty, String.Empty, String.Empty);
        }

        private static Business CreateEmptyBusiness()
        {
            return new Business(string.Empty, String.Empty, String.Empty, string.Empty);
        }

        private static Address CreateEmptyAddress()
        {
            return new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                "United Kingdom");
        }

        [Fact]
        public void ProducersCanOnlyHaveOneSiteOfExport()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var producer1 = notification.AddProducer(CreateEmptyBusiness(), CreateEmptyAddress(), CreateEmptyContact());
            var producer2 = notification.AddProducer(CreateEmptyBusiness(), CreateEmptyAddress(), CreateEmptyContact());

            EntityHelper.SetEntityIds(producer1, producer2);

            notification.SetProducerAsSiteOfExport(producer1.Id);

            var siteOfExportCount = notification.Producers.Count(p => p.IsSiteOfExport);
            Assert.Equal(1, siteOfExportCount);
        }

        [Fact]
        public void CantSetNonExistentProducerAsSiteOfExport()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var producer = notification.AddProducer(CreateEmptyBusiness(), CreateEmptyAddress(), CreateEmptyContact());
            EntityHelper.SetEntityId(producer, new Guid("{D65D91BA-FA77-47F6-ACF5-B1A405DEE187}"));

            var badId = new Guid("{5DF206F6-4116-4EEC-949A-0FC71FE609C1}");

            Action setAsSiteOfExport = () => notification.SetProducerAsSiteOfExport(badId);

            Assert.Throws<InvalidOperationException>(setAsSiteOfExport);
        }

        [Fact]
        public void CantRemoveNonExistentProducer()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action removeProducer = () => notification.RemoveProducer(new Guid("{BD49EF90-C9B2-4E84-B0D3-964BC2A592D5}"));

            Assert.Throws<InvalidOperationException>(removeProducer);
        }

        [Fact]
        public void UpdateProducerModifiesCollection()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            var producerId = new Guid("{D65D91BA-FA77-47F6-ACF5-B1A405DEE187}");

            var producer = notification.AddProducer(CreateEmptyBusiness(), CreateEmptyAddress(), CreateEmptyContact());
            EntityHelper.SetEntityId(producer, producerId);

            var updateProducer = notification.Producers.Single(p => p.Id == producerId);
            var newAddress = new Address("new building", string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty,
                "United Kingdom");

            updateProducer.Address = newAddress;

            Assert.Equal("new building", notification.Producers.Single(p => p.Id == producerId).Address.Building);
        }

        [Fact]
        public void CanAddExporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = CreateEmptyBusiness();
            var address = CreateEmptyAddress();
            var contact = CreateEmptyContact();

            notification.AddExporter(business, address, contact);

            Assert.True(notification.HasExporter);
        }

        [Fact]
        public void CantAddMultipleExporters()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = CreateEmptyBusiness();
            var address = CreateEmptyAddress();
            var contact = CreateEmptyContact();

            notification.AddExporter(business, address, contact);

            Action addSecondExporter = () => notification.AddExporter(business, address, contact);

            Assert.Throws<InvalidOperationException>(addSecondExporter);
        }

        [Fact]
        public void HasExporterDefaultToFalse()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Assert.False(notification.HasExporter);
        }

        [Fact]
        public void RemoveExporterSetsToNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = CreateEmptyBusiness();
            var address = CreateEmptyAddress();
            var contact = CreateEmptyContact();

            notification.AddExporter(business, address, contact);

            notification.RemoveExporter();

            Assert.Null(notification.Exporter);
        }

        [Fact]
        public void CanAddImporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = CreateEmptyBusiness();
            var address = CreateEmptyAddress();
            var contact = CreateEmptyContact();

            notification.AddImporter(business, address, contact);

            Assert.True(notification.HasImporter);
        }

        [Fact]
        public void CantAddMultipleImporters()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = CreateEmptyBusiness();
            var address = CreateEmptyAddress();
            var contact = CreateEmptyContact();

            notification.AddImporter(business, address, contact);

            Action addSecondImporter = () => notification.AddImporter(business, address, contact);

            Assert.Throws<InvalidOperationException>(addSecondImporter);
        }

        [Fact]
        public void HasImporterDefaultToFalse()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Assert.False(notification.HasImporter);
        }

        [Fact]
        public void RemoveImporterSetsToNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = CreateEmptyBusiness();
            var address = CreateEmptyAddress();
            var contact = CreateEmptyContact();

            notification.AddImporter(business, address, contact);

            notification.RemoveImporter();

            Assert.Null(notification.Importer);
        }

        [Fact]
        public void CreateShipmentInfo_WithInvalidEndDate_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(-1); //Invalid end date

            Action addShipmentInfo = () => notification.AddShipmentDatesAndQuantityInfo(startDate, endDate, 10, 0.0001M, ShipmentQuantityUnits.Tonnes);

            Assert.Throws<InvalidOperationException>(addShipmentInfo);
        }

        [Fact]
        public void CreateShipmentInfo_WithInvalidStartDate_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var startDate = DateTime.Now.AddDays(1); //Invalid start date
            var endDate = DateTime.Now; 

            Action addShipmentInfo = () => notification.AddShipmentDatesAndQuantityInfo(startDate, endDate, 10, 0.0001M, ShipmentQuantityUnits.Tonnes);

            Assert.Throws<InvalidOperationException>(addShipmentInfo);
        }

        [Fact]
        public void CreateShipmentInfo_WithValidData_AddsShipmentInfo()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddPackagingInfo(PackagingType.Bulk);

            notification.SetSpecialHandling(false, string.Empty);

            notification.AddShipmentDatesAndQuantityInfo(DateTime.Now, DateTime.Now.AddDays(1), 10, 0.0001M, ShipmentQuantityUnits.Tonnes);

            Assert.True(notification.HasShipmentInfo);
        }

        [Fact]
        public void CreatePackagingInfo_WithInvalidData_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddPackagingInfo(PackagingType.Bulk);

            notification.SetSpecialHandling(false, string.Empty);

            notification.AddShipmentDatesAndQuantityInfo(DateTime.Now, DateTime.Now.AddDays(1), 10, 0.0001M, ShipmentQuantityUnits.Tonnes);

            Action addPackagingInfo = () => notification.ShipmentInfo.AddPackagingInfo(PackagingType.Bag, "Limited Company");

            Assert.Throws<InvalidOperationException>(addPackagingInfo);
        }

        [Fact]
        public void CreatePackagingInfo_WithValidData_PackagingInfoAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddPackagingInfo(PackagingType.Other, "Limited Company");

            notification.SetSpecialHandling(false, string.Empty);

            notification.AddShipmentDatesAndQuantityInfo(DateTime.Now, DateTime.Now.AddDays(1), 10, 0.0001M, ShipmentQuantityUnits.Tonnes);

            Assert.Equal(1, notification.ShipmentInfo.PackagingInfos.Count());
        }

        [Fact]
        public void FacilitiesCanOnlyHaveOneSiteOfTreatment()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var facility1 = notification.AddFacility(CreateEmptyBusiness(), CreateEmptyAddress(), CreateEmptyContact());
            var facility2 = notification.AddFacility(CreateEmptyBusiness(), CreateEmptyAddress(), CreateEmptyContact());

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

            var facility = notification.AddFacility(CreateEmptyBusiness(), CreateEmptyAddress(), CreateEmptyContact());
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

            Action removeFacility = () => notification.RemoveFacility(new Guid("{BD49EF90-C9B2-4E84-B0D3-964BC2A592D5}"));

            Assert.Throws<InvalidOperationException>(removeFacility);
        }
    }
}