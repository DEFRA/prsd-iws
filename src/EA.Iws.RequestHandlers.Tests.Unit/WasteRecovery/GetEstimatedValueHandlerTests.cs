namespace EA.Iws.RequestHandlers.Tests.Unit.WasteRecovery
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using FakeItEasy;
    using RequestHandlers.Mappings;
    using RequestHandlers.WasteRecovery;
    using Requests.WasteRecovery;
    using Xunit;

    public class GetEstimatedValueHandlerTests
    {
        private static readonly Guid NotificationId = Guid.NewGuid();
        private readonly GetEstimatedValueHandler handler;
        private readonly IWasteRecoveryRepository repository;

        public GetEstimatedValueHandlerTests()
        {
            repository = A.Fake<IWasteRecoveryRepository>();

            handler = new GetEstimatedValueHandler(repository, new ValuePerWeightMap());
        }

        [Fact]
        public async Task WasteRecoveryNull_ReturnsNull()
        {
            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns<WasteRecovery>(null);

            var result = await handler.HandleAsync(new GetEstimatedValue(NotificationId));

            Assert.Null(result);
        }

        [Fact]
        public async Task HasWasteRecovery_ReturnsCorrectValues()
        {
            var wasteRecovery = new WasteRecovery(NotificationId,
                new Percentage(100m),
                new EstimatedValue(ValuePerWeightUnits.Kilogram, 10m),
                new RecoveryCost(ValuePerWeightUnits.Kilogram, 15m));

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);

            var result = await handler.HandleAsync(new GetEstimatedValue(NotificationId));

            Assert.Equal(10m, result.Amount);
            Assert.Equal(ValuePerWeightUnits.Kilogram, result.Unit);
        }
    }
}
