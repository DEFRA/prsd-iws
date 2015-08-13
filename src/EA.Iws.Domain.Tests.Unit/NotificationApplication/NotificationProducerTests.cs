namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Linq;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationProducerTests
    {
        private readonly NotificationApplication notification;
        private readonly Producer anyProducer1;
        private readonly Producer anyProducer2;
        private static readonly Guid NonExistentProducerId = new Guid("94C2EF67-B445-41E6-B806-BE30CBBEAF36");
        private static readonly Guid ProducerId = new Guid("1FB44A2C-903F-4EA7-A74B-76BCDF413FFB");

        public NotificationProducerTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            anyProducer1 = notification.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());
            anyProducer2 = notification.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

            EntityHelper.SetEntityId(anyProducer1, new Guid("18C7F135-AE7F-4F1E-B6F9-076C12224292"));
            EntityHelper.SetEntityId(anyProducer2, new Guid("58318441-922C-4453-8A96-D5AA2E3D9B5A"));
        }

        [Fact]
        public void ProducersCanOnlyHaveOneSiteOfExport()
        {
            notification.SetProducerAsSiteOfExport(anyProducer1.Id);

            var siteOfExportCount = notification.Producers.Count(p => p.IsSiteOfExport);

            Assert.Equal(1, siteOfExportCount);
        }

        [Fact]
        public void CantSetNonExistentProducerAsSiteOfExport()
        {
            Action setAsSiteOfExport = () => notification.SetProducerAsSiteOfExport(NonExistentProducerId);

            Assert.Throws<InvalidOperationException>(setAsSiteOfExport);
        }

        [Fact]
        public void CantRemoveNonExistentProducer()
        {
            Action removeProducer =
                () => notification.RemoveProducer(NonExistentProducerId);

            Assert.Throws<InvalidOperationException>(removeProducer);
        }

        [Fact]
        public void UpdateProducerModifiesCollection()
        {
            var updateProducer = notification.Producers.Single(p => p.Id == anyProducer1.Id);

            var newAddress1 = "new address one";

            var newAddress = new Address(newAddress1, string.Empty, "town", string.Empty, string.Empty, "country");

            updateProducer.Address = newAddress;

            Assert.Equal(newAddress1, notification.Producers.Single(p => p.Id == anyProducer1.Id).Address.Address1);
        }

        [Fact]
        public void RemoveExistingProducerByProducerId()
        {
            Producer tempProducer = notification.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempProducer, ProducerId);

            int beforeProducersCount = notification.Producers.Count();
            notification.RemoveProducer(ProducerId);
            int afterProducersCount = notification.Producers.Count();

            Assert.True(afterProducersCount == beforeProducersCount - 1);
        }

        [Fact]
        public void CanRemoveProducerOtherThanSiteOfExport()
        {
            Producer tempProducer = notification.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempProducer, ProducerId);

            int beforeProducersCount = notification.Producers.Count();
            notification.RemoveProducer(ProducerId);
            Assert.True(notification.Producers.Count() == beforeProducersCount - 1);
        }

        [Fact]
        public void CannotRemoveExistingSiteOfExportProducerByProducerIdForMoreThanOneProducers()
        {
            Producer tempProducer = notification.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempProducer, ProducerId);
            notification.SetProducerAsSiteOfExport(ProducerId);
            notification.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                                     ObjectFactory.CreateDefaultAddress(),
                                     ObjectFactory.CreateEmptyContact());
            Assert.True(notification.Producers.Count() == 4);
            Assert.True(notification.Producers.Count(x => x.IsSiteOfExport) == 1);

            Action removeSiteOfExportProducer = (() => notification.RemoveProducer(ProducerId));
            Assert.Throws<InvalidOperationException>(removeSiteOfExportProducer);
        }
    }
}