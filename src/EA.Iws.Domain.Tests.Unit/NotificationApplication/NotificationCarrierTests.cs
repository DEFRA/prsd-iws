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
        private readonly NotificationApplication notification;

        private int CarriersCount
        {
            get { return notification.Carriers.Count(); }
        }

        public NotificationCarrierTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);
        }

        private void AddCarrier()
        {
            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddCarrier(business, address, contact);
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
            EntityHelper.SetEntityId(notification.Carriers.First(), carrierId);

            notification.RemoveCarrier(carrierId);
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
            foreach (var carrier in notification.Carriers)
            {
                EntityHelper.SetEntityId(carrier, carrierIds[j]);
                j++;
            }
            Assert.Equal(CarriersCount, 5);

            for (int k = 0; k < 5; k++)
            {
                notification.RemoveCarrier(carrierIds[k]);
            }
            Assert.Equal(CarriersCount, 0);
        }
    }
}