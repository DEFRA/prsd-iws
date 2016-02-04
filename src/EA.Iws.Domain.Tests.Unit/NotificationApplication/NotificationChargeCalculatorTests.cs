namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using Finance;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class NotificationChargeCalculatorTests
    {
        private const decimal NotificationPrice = 25.75m;
        private const decimal NotificationSetPrice = 1234.56m;
        private const int LowerRange = 5;
        private const int MidRange = 10;
        private const int UpperRange = 15;

        private readonly Guid notificationId;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly INotificationChargeCalculator chargeCalculator;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IPricingStructureRepository pricingStructureRepository;
        private readonly ShipmentInfo shipmentInfo;
        private readonly TestableNotificationApplication notificationApplication;
        private readonly IFacilityRepository facilityRepository;

        public NotificationChargeCalculatorTests()
        {
            notificationId = new Guid("C4C62654-048C-45A2-BF7F-9837EFCF328F");

            shipmentInfoRepository = A.Fake<IShipmentInfoRepository>();
            shipmentInfo = A.Fake<ShipmentInfo>();
            notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();
            pricingStructureRepository = A.Fake<IPricingStructureRepository>();
            facilityRepository = A.Fake<IFacilityRepository>();

            notificationApplication = new TestableNotificationApplication();

            chargeCalculator = new NotificationChargeCalculator(shipmentInfoRepository, notificationApplicationRepository, pricingStructureRepository, facilityRepository);
        }

        [Fact]
        public async Task ShipmentInfoNull_ReturnsZero()
        {
            A.CallTo(() => shipmentInfoRepository.GetByNotificationId(notificationId)).Returns<ShipmentInfo>(null);

            var result = await chargeCalculator.GetValue(notificationId);

            Assert.Equal(0m, result);
        }

        [Fact]
        public async Task ChargeSet_ReturnsSetCharge()
        {
            SetupNotification();
            ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.NumberOfShipments, MidRange, shipmentInfo);
            A.CallTo(() => shipmentInfoRepository.GetByNotificationId(notificationId)).Returns(shipmentInfo);
            A.CallTo(() => notificationApplicationRepository.GetById(notificationId)).Returns(notificationApplication);
            A.CallTo(() => pricingStructureRepository.Get()).Returns(GetPricingStructures());

            notificationApplication.SetCharge(NotificationSetPrice);

            var result = await chargeCalculator.GetValue(notificationId);

            Assert.Equal(NotificationSetPrice, result);
        }

        [Fact]
        public async Task ChargeNotSet_CalculatesCharge()
        {
            SetupNotification();
            ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.NumberOfShipments, MidRange, shipmentInfo);
            A.CallTo(() => shipmentInfoRepository.GetByNotificationId(notificationId)).Returns(shipmentInfo);
            A.CallTo(() => notificationApplicationRepository.GetById(notificationId)).Returns(notificationApplication);
            A.CallTo(() => pricingStructureRepository.Get()).Returns(GetPricingStructures());
            A.CallTo(() => facilityRepository.GetByNotificationId(notificationId)).Returns(GetFacilityCollection());

            var result = await chargeCalculator.GetValue(notificationId);

            Assert.Equal(NotificationPrice, result);
        }

        private IEnumerable<PricingStructure> GetPricingStructures()
        {
            var pricingStructure = ObjectInstantiator<PricingStructure>.CreateNew();

            ObjectInstantiator<PricingStructure>.SetProperty(x => x.CompetentAuthority, CompetentAuthorityEnum.England, pricingStructure);

            var activity = A.Fake<Activity>();
            ObjectInstantiator<Activity>.SetProperty(x => x.TradeDirection, TradeDirection.Export, activity);
            ObjectInstantiator<Activity>.SetProperty(x => x.NotificationType, NotificationType.Recovery, activity);
            ObjectInstantiator<Activity>.SetProperty(x => x.IsInterim, false, activity);

            ObjectInstantiator<PricingStructure>.SetProperty(x => x.Activity, activity, pricingStructure);

            var shipmentQuantityRange = A.Fake<ShipmentQuantityRange>();
            ObjectInstantiator<ShipmentQuantityRange>.SetProperty(x => x.RangeFrom, LowerRange, shipmentQuantityRange);
            ObjectInstantiator<ShipmentQuantityRange>.SetProperty(x => x.RangeTo, UpperRange, shipmentQuantityRange);

            ObjectInstantiator<PricingStructure>.SetProperty(x => x.ShipmentQuantityRange, shipmentQuantityRange, pricingStructure);

            ObjectInstantiator<PricingStructure>.SetProperty(x => x.Price, NotificationPrice, pricingStructure);

            var p = new List<PricingStructure>
            {
                pricingStructure
            };

            return p;
        }

        private void SetupNotification()
        {
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.CompetentAuthority, CompetentAuthorityEnum.England, notificationApplication);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.NotificationType, NotificationType.Recovery, notificationApplication);
        }

        private FacilityCollection GetFacilityCollection()
        {
            var facilityCollection = new FacilityCollection(notificationId);
            facilityCollection.AddFacility(new TestableBusiness(), new TestableAddress(), new TestableContact());
            facilityCollection.AddFacility(new TestableBusiness(), new TestableAddress(), new TestableContact());
            return facilityCollection;
        }
    }
}
