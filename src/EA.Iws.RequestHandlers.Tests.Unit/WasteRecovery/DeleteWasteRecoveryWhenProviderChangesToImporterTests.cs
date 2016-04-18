namespace EA.Iws.RequestHandlers.Tests.Unit.WasteRecovery
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication.WasteRecovery;
    using FakeItEasy;
    using RequestHandlers.WasteRecovery;
    using Xunit;

    public class DeleteWasteRecoveryWhenProviderChangesToImporterTests
    {
        private readonly TestIwsContext context;
        private readonly IWasteRecoveryRepository recoveryRepository;
        private readonly IWasteDisposalRepository disposalRepository;
        private readonly DeleteWasteRecoveryWhenProviderChangesToImporter handler;
        private static readonly Guid NotificationId = Guid.NewGuid();

        public DeleteWasteRecoveryWhenProviderChangesToImporterTests()
        {
            context = new TestIwsContext();
            recoveryRepository = A.Fake<IWasteRecoveryRepository>();
            disposalRepository = A.Fake<IWasteDisposalRepository>();

            handler = new DeleteWasteRecoveryWhenProviderChangesToImporter(context, recoveryRepository, disposalRepository);
        }

        [Fact]
        public async Task WasteRecoveryExists_WasteDisposalNull_ChangeToImporter_DeletesWasteRecovery()
        {
            var wasteRecovery = new WasteRecovery(NotificationId, null, null, null);

            A.CallTo(() => recoveryRepository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);
            A.CallTo(() => disposalRepository.GetByNotificationId(NotificationId)).Returns<WasteDisposal>(null);

            await handler.HandleAsync(new ProviderChangedEvent(NotificationId, ProvidedBy.Importer));

            A.CallTo(() => recoveryRepository.Delete(wasteRecovery)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task WasteRecoveryExists_WasteDisposalExists_ChangeToImporter_DeletesBoth()
        {
            var wasteRecovery = new WasteRecovery(NotificationId, null, null, null);
            var wasteDisposal = new WasteDisposal(NotificationId, "Test", null);

            A.CallTo(() => recoveryRepository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);
            A.CallTo(() => disposalRepository.GetByNotificationId(NotificationId)).Returns(wasteDisposal);

            await handler.HandleAsync(new ProviderChangedEvent(NotificationId, ProvidedBy.Importer));

            A.CallTo(() => recoveryRepository.Delete(wasteRecovery)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => disposalRepository.Delete(wasteDisposal)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task IfWasteRecoveryDeleted_CallsSaveChanges()
        {
            var wasteRecovery = new WasteRecovery(NotificationId, null, null, null);

            A.CallTo(() => recoveryRepository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);

            await handler.HandleAsync(new ProviderChangedEvent(NotificationId, ProvidedBy.Importer));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task IfBothDeleted_CallsSaveChanges()
        {
            var wasteRecovery = new WasteRecovery(NotificationId, null, null, null);
            var wasteDisposal = new WasteDisposal(NotificationId, "Test", null);

            A.CallTo(() => recoveryRepository.GetByNotificationId(NotificationId)).Returns(wasteRecovery);
            A.CallTo(() => disposalRepository.GetByNotificationId(NotificationId)).Returns(wasteDisposal);

            await handler.HandleAsync(new ProviderChangedEvent(NotificationId, ProvidedBy.Importer));

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}
