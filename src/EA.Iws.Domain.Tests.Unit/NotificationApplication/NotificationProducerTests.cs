namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Linq;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationProducerTests
    {
        private readonly ProducerCollection producerCollection;
        private readonly Producer anyProducer1;
        private readonly Producer anyProducer2;
        private static readonly Guid NonExistentProducerId = new Guid("94C2EF67-B445-41E6-B806-BE30CBBEAF36");
        private static readonly Guid ProducerId = new Guid("1FB44A2C-903F-4EA7-A74B-76BCDF413FFB");

        public NotificationProducerTests()
        {
            producerCollection = new ProducerCollection(new Guid("784C0850-7FBA-401D-BA0F-64600B36583C"));

            anyProducer1 = producerCollection.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());
            anyProducer2 = producerCollection.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

            EntityHelper.SetEntityId(anyProducer1, new Guid("18C7F135-AE7F-4F1E-B6F9-076C12224292"));
            EntityHelper.SetEntityId(anyProducer2, new Guid("58318441-922C-4453-8A96-D5AA2E3D9B5A"));
        }

        [Fact]
        public void ProducersCanOnlyHaveOneSiteOfExport()
        {
            producerCollection.SetProducerAsSiteOfExport(anyProducer1.Id);

            var siteOfExportCount = producerCollection.Producers.Count(p => p.IsSiteOfExport);

            Assert.Equal(1, siteOfExportCount);
        }

        [Fact]
        public void CantSetNonExistentProducerAsSiteOfExport()
        {
            Action setAsSiteOfExport = () => producerCollection.SetProducerAsSiteOfExport(NonExistentProducerId);

            Assert.Throws<InvalidOperationException>(setAsSiteOfExport);
        }

        [Fact]
        public void CantRemoveNonExistentProducer()
        {
            Action removeProducer =
                () => producerCollection.RemoveProducer(NonExistentProducerId);

            Assert.Throws<InvalidOperationException>(removeProducer);
        }

        [Fact]
        public void UpdateProducerModifiesCollection()
        {
            var updateProducer = producerCollection.Producers.Single(p => p.Id == anyProducer1.Id);

            var newAddress1 = "new address one";

            var newAddress = new Address(newAddress1, string.Empty, "town", string.Empty, string.Empty, "country");

            updateProducer.Address = newAddress;

            Assert.Equal(newAddress1, producerCollection.Producers.Single(p => p.Id == anyProducer1.Id).Address.Address1);
        }

        [Fact]
        public void RemoveExistingProducerByProducerId()
        {
            Producer tempProducer = producerCollection.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempProducer, ProducerId);

            int beforeProducersCount = producerCollection.Producers.Count();
            producerCollection.RemoveProducer(ProducerId);
            int afterProducersCount = producerCollection.Producers.Count();

            Assert.True(afterProducersCount == beforeProducersCount - 1);
        }

        [Fact]
        public void CanRemoveProducerOtherThanSiteOfExport()
        {
            Producer tempProducer = producerCollection.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempProducer, ProducerId);

            int beforeProducersCount = producerCollection.Producers.Count();
            producerCollection.RemoveProducer(ProducerId);
            Assert.True(producerCollection.Producers.Count() == beforeProducersCount - 1);
        }

        [Fact]
        public void CannotRemoveExistingSiteOfExportProducerByProducerIdForMoreThanOneProducers()
        {
            Producer tempProducer = producerCollection.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                                                             ObjectFactory.CreateDefaultAddress(),
                                                             ObjectFactory.CreateEmptyContact());
            EntityHelper.SetEntityId(tempProducer, ProducerId);
            producerCollection.SetProducerAsSiteOfExport(ProducerId);
            producerCollection.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                                     ObjectFactory.CreateDefaultAddress(),
                                     ObjectFactory.CreateEmptyContact());
            Assert.True(producerCollection.Producers.Count() == 4);
            Assert.True(producerCollection.Producers.Count(x => x.IsSiteOfExport) == 1);

            Action removeSiteOfExportProducer = (() => producerCollection.RemoveProducer(ProducerId));
            Assert.Throws<InvalidOperationException>(removeSiteOfExportProducer);
        }
    }
}