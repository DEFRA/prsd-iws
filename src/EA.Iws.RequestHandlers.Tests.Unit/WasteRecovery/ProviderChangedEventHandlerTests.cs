namespace EA.Iws.RequestHandlers.Tests.Unit.WasteRecovery
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication.WasteRecovery;
    using FakeItEasy;
    using RequestHandlers.WasteRecovery;
    using Xunit;

    public class ProviderChangedEventHandlerTests
    {
        private readonly TestIwsContext context;
        private readonly IWasteRecoveryRepository repository;
        private readonly ProviderChangedEventHandler handler;
        private static readonly Guid NotificationId = Guid.NewGuid();

        public ProviderChangedEventHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<IWasteRecoveryRepository>();
            
            handler = new ProviderChangedEventHandler(context, repository);
        }

        [Fact]
        public async Task WasteRecoveryExists_ChangeToImporter_DeletesRecoveryInfo()
        {
            var wasteRecovery = new WasteRecovery(NotificationId, null, null, null);

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);

            await handler.HandleAsync(new ProviderChangedEvent(NotificationId, ProvidedBy.Importer));

            A.CallTo(() => repository.Delete(wasteRecovery)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task IfWasteRecoveryDeleted_CallsSaveChanges()
        {
            var wasteRecovery = new WasteRecovery(NotificationId, null, null, null);

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);

            await handler.HandleAsync(new ProviderChangedEvent(NotificationId, ProvidedBy.Importer));

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}
