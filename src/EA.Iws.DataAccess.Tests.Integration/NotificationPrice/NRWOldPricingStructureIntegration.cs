namespace EA.Iws.DataAccess.Tests.Integration.NotificationPrice
{
    using Core.Notification;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    [Trait("Category", "Integration")]
    public class NRWOldPricingStructureIntegration
    {
        private readonly IwsContext context;

        public NRWOldPricingStructureIntegration()
        {
            var userContext = A.Fake<IUserContext>();
            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());
            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
        }

        [Theory]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 1, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 2, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 5, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 6, 2700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 20, 2700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 21, 4070)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 100, 4070)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 101, 7920)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 500, 7920)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 501, 14380)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 1, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 2, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 5, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 6, 2700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 20, 2700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 21, 4070)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 100, 4070)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 101, 7920)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 500, 7920)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 501, 14380)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 1, 1540)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 2, 1540)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 5, 1540)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 6, 3330)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 20, 3330)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 21, 5500)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 100, 5500)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 101, 10600)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 500, 10600)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 501, 19500)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 1, 1700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 2, 1700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 5, 1700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 6, 3330)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 20, 3330)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 21, 6000)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 100, 6000)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 101, 12900)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 500, 12900)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 501, 24000)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 1, 1250)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 2, 1250)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 5, 1250)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 6, 2700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 20, 2700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 21, 4900)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 100, 4900)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 101, 10600)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 500, 10600)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 501, 19500)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 1, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 2, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 5, 1450)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 6, 2830)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 20, 2830)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 21, 5500)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 100, 5500)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 101, 12900)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 500, 12900)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 501, 24000)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 1, 1540)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 2, 1540)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 5, 1540)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 6, 3330)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 20, 3330)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 21, 5500)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 100, 5500)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 101, 10600)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 500, 10600)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 501, 19500)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 1, 1700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 2, 1700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 5, 1700)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 6, 3330)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 20, 3330)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 21, 6000)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 100, 6000)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 101, 12900)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 500, 12900)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 501, 24000)]
        public async Task PricingStructureCorrectValue(UKCompetentAuthority ca, TradeDirection td, int nt,
                                                       bool isInterim, int numberOfShipments, int expectedPrice)
        {
            var result = await context.PricingStructures
                                      .OrderByDescending(ps => ps.ValidFrom)
                                      .Where(p => p.CompetentAuthority == ca &&
                                             p.Activity.TradeDirection == td &&
                                             (int)p.Activity.NotificationType == nt &&
                                             p.Activity.IsInterim == isInterim &&
                                             p.ValidFrom <= new DateTime(2023, 1, 1) &&
                                             p.ShipmentQuantityRange.RangeFrom <= numberOfShipments &&
                                             (p.ShipmentQuantityRange.RangeTo == null || p.ShipmentQuantityRange.RangeTo >= numberOfShipments))
                                      .FirstOrDefaultAsync();

            Assert.Equal(expectedPrice, result.Price);
        }
    }
}
