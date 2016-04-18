namespace EA.Iws.RequestHandlers.Tests.Unit.WasteRecovery
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using FakeItEasy;
    using RequestHandlers.WasteRecovery;
    using Requests.WasteRecovery;
    using Xunit;

    public class GetRecoverablePercentageHandlerTests
    {
        private static readonly Guid NotificationId = Guid.NewGuid();
        private readonly GetRecoverablePercentageHandler handler;
        private readonly IWasteRecoveryRepository repository;

        public GetRecoverablePercentageHandlerTests()
        {
            repository = A.Fake<IWasteRecoveryRepository>();

            handler = new GetRecoverablePercentageHandler(repository);
        }

        [Fact]
        public async Task NoWasteRecovery_ReturnsNull()
        {
            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns<WasteRecovery>(null);

            var result = await handler.HandleAsync(new GetRecoverablePercentage(NotificationId));

            Assert.Null(result);
        }

        [Fact]
        public async Task WasteRecoveryExists_ReturnsPercentage()
        {
            var wasteRecovery = new WasteRecovery(NotificationId,
                new Percentage(100m),
                new EstimatedValue(ValuePerWeightUnits.Kilogram, 5m),
                new RecoveryCost(ValuePerWeightUnits.Kilogram, 10m));

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);

            var result = await handler.HandleAsync(new GetRecoverablePercentage(NotificationId));

            Assert.Equal(100m, result);
        }
    }
}
