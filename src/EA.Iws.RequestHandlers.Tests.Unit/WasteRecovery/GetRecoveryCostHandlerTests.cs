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

    public class GetRecoveryCostHandlerTests
    {
        private readonly GetRecoveryCostHandler handler;
        private readonly IWasteRecoveryRepository repository;
        private static readonly Guid NotificationId = Guid.NewGuid();

        public GetRecoveryCostHandlerTests()
        {
            repository = A.Fake<IWasteRecoveryRepository>();

            handler = new GetRecoveryCostHandler(repository, new ValuePerWeightMap());
        }

        [Fact]
        public async Task WasteRecoveryNull_ReturnsNull()
        {
            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns<WasteRecovery>(null);

            var result = await handler.HandleAsync(new GetRecoveryCost(NotificationId));

            Assert.Null(result);
        }

        [Fact]
        public async Task HasWasteRecovery_ReturnsCorrectValues()
        {
            var wasteRecovery = new WasteRecovery(
                NotificationId,
                new Percentage(100m),
                new EstimatedValue(ValuePerWeightUnits.Kilogram, 5m),
                new RecoveryCost(ValuePerWeightUnits.Tonne, 10m));

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);

            var result = await handler.HandleAsync(new GetRecoveryCost(NotificationId));

            Assert.Equal(10m, result.Amount);
            Assert.Equal(ValuePerWeightUnits.Tonne, result.Unit);
        }
    }
}
