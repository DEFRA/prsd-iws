namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using Helpers;
    using Xunit;

    public class NotificationProducerTests
    {
        [Fact]
        public void ProducersCanOnlyHaveOneSiteOfExport()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var producer1 = notification.AddProducer(ObjectFactory.CreateEmptyBusiness(),
                ObjectFactory.CreateEmptyAddress(), ObjectFactory.CreateEmptyContact());
            var producer2 = notification.AddProducer(ObjectFactory.CreateEmptyBusiness(),
                ObjectFactory.CreateEmptyAddress(), ObjectFactory.CreateEmptyContact());

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

            var producer = notification.AddProducer(ObjectFactory.CreateEmptyBusiness(),
                ObjectFactory.CreateEmptyAddress(), ObjectFactory.CreateEmptyContact());
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

            Action removeProducer =
                () => notification.RemoveProducer(new Guid("{BD49EF90-C9B2-4E84-B0D3-964BC2A592D5}"));

            Assert.Throws<InvalidOperationException>(removeProducer);
        }

        [Fact]
        public void UpdateProducerModifiesCollection()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            var producerId = new Guid("{D65D91BA-FA77-47F6-ACF5-B1A405DEE187}");

            var producer = notification.AddProducer(ObjectFactory.CreateEmptyBusiness(),
                ObjectFactory.CreateEmptyAddress(), ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(producer, producerId);

            var updateProducer = notification.Producers.Single(p => p.Id == producerId);
            var newAddress = new Address("new building", string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty,
                "United Kingdom");

            updateProducer.Address = newAddress;

            Assert.Equal("new building", notification.Producers.Single(p => p.Id == producerId).Address.Building);
        }
    }
}