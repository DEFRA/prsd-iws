namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationCarrierTests
    {
        private readonly Guid carrierId = Guid.NewGuid();
        private readonly CarrierCollection carrierCollection;
        private static readonly Guid NotificationId = new Guid("0F458A12-2DD6-48A6-8ABF-D000D178D4C8");

        private int CarriersCount
        {
            get { return carrierCollection.Carriers.Count(); }
        }

        public NotificationCarrierTests()
        {
            carrierCollection = new CarrierCollection(NotificationId);
        }

        private void AddCarrier()
        {
            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            carrierCollection.AddCarrier(business, address, contact);
        }

        [Fact]
        public void CanAddCarrier()
        {
            AddCarrier();
            Assert.Equal(CarriersCount, 1);
        }

        [Fact]
        public void CanAddMultipleCarriers()
        {
            for (int i = 0; i < 5; i++)
            {
                AddCarrier();
            }

            Assert.Equal(CarriersCount, 5);
        }

        [Fact]
        public void CanRemoveCarrier()
        {
            AddCarrier();
            EntityHelper.SetEntityId(carrierCollection.Carriers.First(), carrierId);

            carrierCollection.RemoveCarrier(carrierId);
            Assert.Equal(CarriersCount, 0);
        }

        [Fact]
        public void CanRemoveAllCarriersForNotification()
        {
            List<Guid> carrierIds = new List<Guid>();
            for (int i = 0; i < 5; i++)
            {
                AddCarrier();
                carrierIds.Add(Guid.NewGuid());
            }

            int j = 0;
            foreach (var carrier in carrierCollection.Carriers)
            {
                EntityHelper.SetEntityId(carrier, carrierIds[j]);
                j++;
            }
            Assert.Equal(CarriersCount, 5);

            for (int k = 0; k < 5; k++)
            {
                carrierCollection.RemoveCarrier(carrierIds[k]);
            }
            Assert.Equal(CarriersCount, 0);
        }
    }
}