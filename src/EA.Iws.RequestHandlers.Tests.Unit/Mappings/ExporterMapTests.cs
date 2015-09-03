namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using System;
    using Core.Shared;
    using RequestHandlers.Mappings;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ExporterMapTests : BusinessTestBase
    {
        private readonly ExporterDataMap exporterMap;
        private readonly TestableExporter exporter;

        public ExporterMapTests()
        {
            exporter = new TestableExporter
            {
                Id = Guid1
            };
            exporterMap = new ExporterDataMap(addressMap, businessMap, contactMap);
        }

        [Fact]
        public void MapReturnsCorrectAddress()
        {
            var result = exporterMap.Map(exporter);

            Assert.Equal(ReturnAddress, result.Address, AddressComparer);
        }
        
        [Fact]
        public void MapSetsIdAndHasExporter()
        {
            var result = exporterMap.Map(exporter);

            Assert.Equal(exporter.Id, result.Id);
            Assert.True(result.HasExporter);
        }

        [Fact]
        public void MapReturnsCorrectBusiness()
        {
            var result = exporterMap.Map(exporter);

            Assert.Equal(ReturnBusiness, result.Business, BusinessComparer);
        }

        [Fact]
        public void MapReturnsCorrectContact()
        {
            var result = exporterMap.Map(exporter);

            Assert.Equal(ReturnContact, result.Contact, ContactComparer);
        }

        [Fact]
        public void MapReturnsBlankNotificationId()
        {
            var result = exporterMap.Map(exporter);

            Assert.Equal(Guid.Empty, result.NotificationId);
        }

        [Fact]
        public void MapWithNotificationIdReturnsCorrectId()
        {
            var result = exporterMap.Map(exporter, Guid1);

            Assert.Equal(Guid1, result.NotificationId);
        }

        [Fact]
        public void MapWithNotificationNoExporterReturnsEmptyData()
        {
            ReturnBusiness = null;
            ReturnAddress = null;
            ReturnContact = null;

            var notification = new TestableNotificationApplication
            {
                Id = Guid1
            };

            var result = exporterMap.Map(notification);

            Assert.Equal(Guid1, result.NotificationId);
            Assert.False(result.HasExporter);
            Assert.Equal(new AddressData(), result.Address, AddressComparer);
            Assert.Equal(new BusinessInfoData(), result.Business, BusinessComparer);
            Assert.Equal(new ContactData(), result.Contact, ContactComparer);
        }

        [Fact]
        public void MapWithNotificationWithExporterReturnsData()
        {
            var notification = new TestableNotificationApplication
            {
                Id = Guid1,
                Exporter = new TestableExporter
                {
                    Id = Guid2
                }
            };

            var result = exporterMap.Map(notification);

            Assert.Equal(Guid1, result.NotificationId);
            Assert.Equal(Guid2, result.Id);
            Assert.Equal(ReturnAddress, result.Address, AddressComparer);
            Assert.Equal(ReturnBusiness, result.Business, BusinessComparer);
            Assert.Equal(ReturnContact, result.Contact, ContactComparer);
            Assert.True(result.HasExporter);
        }
    }
}
