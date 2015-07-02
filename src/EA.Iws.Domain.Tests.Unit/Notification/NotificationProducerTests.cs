namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationProducerTests
    {
        private readonly NotificationApplication notification;
        private readonly Producer anyProducer1;
        private readonly Producer anyProducer2;
        private static readonly Guid NonExistentProducerId = new Guid("94C2EF67-B445-41E6-B806-BE30CBBEAF36");

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

            var newBuilding = "new building";

            var newAddress = new Address(newBuilding, "address1", string.Empty, "town", string.Empty,
                string.Empty,
                "country");

            updateProducer.Address = newAddress;

            Assert.Equal(newBuilding, notification.Producers.Single(p => p.Id == anyProducer1.Id).Address.Building);
        }
    }
}