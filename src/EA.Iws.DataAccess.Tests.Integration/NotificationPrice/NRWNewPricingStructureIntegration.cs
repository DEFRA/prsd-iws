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
    public class NRWNewPricingStructureIntegration
    {
        private readonly IwsContext context;

        public NRWNewPricingStructureIntegration()
        {
            var userContext = A.Fake<IUserContext>();
            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());
            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
        }

        [Theory]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 1, 1537)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 2, 1537)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 5, 1537)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 6, 2862)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 20, 2862)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 21, 4314)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 100, 4314)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 101, 8395)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 500, 8395)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, false, 501, 15243)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 1, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 2, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 5, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 6, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 20, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 21, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 100, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 101, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 500, 0)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Recovery, true, 501, 0)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 1, 1632)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 2, 1632)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 5, 1632)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 6, 3530)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 20, 3530)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 21, 5830)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 100, 5830)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 101, 11236)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 500, 11236)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, false, 501, 20670)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 1, 1802)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 2, 1802)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 5, 1802)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 6, 3530)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 20, 3530)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 21, 6360)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 100, 6360)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 101, 13674)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 500, 13674)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Export, (int)NotificationType.Disposal, true, 501, 25440)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 1, 1325)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 2, 1325)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 5, 1325)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 6, 2862)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 20, 2862)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 21, 5194)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 100, 5194)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 101, 11236)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 500, 11236)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, false, 501, 20670)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 1, 1537)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 2, 1537)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 5, 1537)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 6, 3000)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 20, 3000)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 21, 5830)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 100, 5830)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 101, 13674)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 500, 13674)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Recovery, true, 501, 25440)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 1, 1632)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 2, 1632)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 5, 1632)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 6, 3530)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 20, 3530)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 21, 5830)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 100, 5830)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 101, 11236)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 500, 11236)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, false, 501, 20670)]

        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 1, 1802)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 2, 1802)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 5, 1802)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 6, 3530)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 20, 3530)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 21, 6360)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 100, 6360)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 101, 13674)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 500, 13674)]
        [InlineData(UKCompetentAuthority.Wales, TradeDirection.Import, (int)NotificationType.Disposal, true, 501, 25440)]
        public async Task PricingStructureCorrectValue(UKCompetentAuthority ca, TradeDirection td, int nt,
                                                       bool isInterim, int numberOfShipments, int expectedPrice)
        {
            var result = await context.PricingStructures
                                      .OrderByDescending(ps => ps.ValidFrom)
                                      .Where(p => p.CompetentAuthority == ca &&
                                             p.Activity.TradeDirection == td &&
                                             (int)p.Activity.NotificationType == nt &&
                                             p.Activity.IsInterim == isInterim &&
                                             p.ValidFrom <= new DateTime(2025, 04, 15) &&
                                             p.ShipmentQuantityRange.RangeFrom <= numberOfShipments &&
                                             (p.ShipmentQuantityRange.RangeTo == null || p.ShipmentQuantityRange.RangeTo >= numberOfShipments))
                                      .FirstOrDefaultAsync();

            Assert.Equal(expectedPrice, result.Price);
        }
    }
}
