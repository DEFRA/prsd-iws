namespace EA.Iws.RequestHandlers.Tests.Unit.WasteRecovery
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using FakeItEasy;
    using RequestHandlers.WasteRecovery;
    using Requests.WasteRecovery;
    using Xunit;

    public class SaveWasteRecoveryHandlerTests
    {
        private readonly SaveWasteRecoveryHandler handler;
        private readonly TestIwsContext context;
        private readonly IWasteRecoveryRepository repository;
        private static readonly Guid NotificationId = Guid.NewGuid();

        public SaveWasteRecoveryHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<IWasteRecoveryRepository>();
            handler = new SaveWasteRecoveryHandler(repository, context);
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            await handler.HandleAsync(CreateRequest());

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task CreatesNewWasteRecovery()
        {
            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns<WasteRecovery>(null);

            await handler.HandleAsync(CreateRequest());

            var result = context.WasteRecoveries.SingleOrDefault(wr => wr.NotificationId == NotificationId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdatesExistingWasteRecovery()
        {
            var wasteRecovery = new WasteRecovery(
                NotificationId, 
                new Percentage(100m), 
                new EstimatedValue(ValuePerWeightUnits.Tonne, 5m), 
                new RecoveryCost(ValuePerWeightUnits.Kilogram, 7m));

            context.WasteRecoveries.Add(wasteRecovery);
            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);

            await handler.HandleAsync(CreateRequest());

            var result = context.WasteRecoveries.Single(wr => wr.NotificationId == NotificationId);

            Assert.Equal(10m, result.EstimatedValue.Amount);
            Assert.Equal(ValuePerWeightUnits.Kilogram, result.EstimatedValue.Units);

            Assert.Equal(15m, result.RecoveryCost.Amount);
            Assert.Equal(ValuePerWeightUnits.Tonne, result.RecoveryCost.Units);
        }

        private SaveWasteRecovery CreateRequest()
        {
            return new SaveWasteRecovery(
                NotificationId,
                100m,
                new ValuePerWeightData(10m, ValuePerWeightUnits.Kilogram),
                new ValuePerWeightData(15m, ValuePerWeightUnits.Tonne));
        }
    }
}
