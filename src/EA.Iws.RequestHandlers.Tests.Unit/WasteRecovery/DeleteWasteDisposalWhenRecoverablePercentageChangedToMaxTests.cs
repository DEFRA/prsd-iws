namespace EA.Iws.RequestHandlers.Tests.Unit.WasteRecovery
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using FakeItEasy;
    using RequestHandlers.WasteRecovery;
    using Xunit;

    public class DeleteWasteDisposalWhenRecoverablePercentageChangedToMaxTests
    {
        private readonly TestIwsContext context;
        private readonly IWasteDisposalRepository repository;
        private readonly DeleteWasteDisposalWhenRecoverablePercentageChangedToMax handler;
        private static readonly Guid NotificationId = Guid.NewGuid();

        public DeleteWasteDisposalWhenRecoverablePercentageChangedToMaxTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<IWasteDisposalRepository>();
            handler = new DeleteWasteDisposalWhenRecoverablePercentageChangedToMax(context, repository);
        }

        [Fact]
        public async Task PercentageChangedTo100_DeletesDisposal()
        {
            var wasteDisposal = new WasteDisposal(NotificationId, "Test", new DisposalCost(ValuePerWeightUnits.Kilogram, 10m));

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteDisposal);

            await handler.HandleAsync(new PercentageChangedEvent(NotificationId, new Percentage(100)));

            A.CallTo(() => repository.Delete(wasteDisposal)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PercentageChangedToLessThan100_DoesNotDeleteDisposal()
        {
            var wasteDisposal = new WasteDisposal(NotificationId, "Test", new DisposalCost(ValuePerWeightUnits.Kilogram, 10m));

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteDisposal);

            await handler.HandleAsync(new PercentageChangedEvent(NotificationId, new Percentage(50)));

            A.CallTo(() => repository.Delete(A<WasteDisposal>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task DisposalDeleted_CallsSaveChanges()
        {
            var wasteDisposal = new WasteDisposal(NotificationId, "Test", new DisposalCost(ValuePerWeightUnits.Kilogram, 10m));

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteDisposal);

            await handler.HandleAsync(new PercentageChangedEvent(NotificationId, new Percentage(100)));

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}
