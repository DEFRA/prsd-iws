namespace EA.Iws.RequestHandlers.Tests.Unit.RecoveryInfo
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.NotificationApplication.Recovery;
    using FakeItEasy;
    using RequestHandlers.RecoveryInfo;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ProviderChangedEventHandlerTests
    {
        private readonly TestIwsContext context;
        private readonly IRecoveryInfoRepository repository;
        private readonly ProviderChangedEventHandler handler;
        private static readonly Guid NotificationId = Guid.NewGuid();

        public ProviderChangedEventHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<IRecoveryInfoRepository>();
            
            handler = new ProviderChangedEventHandler(context, repository);
        }

        [Fact]
        public async Task RecoveryInfoExists_ChangeToImporter_DeletesRecoveryInfo()
        {
            var recoveryInfo = new RecoveryInfo(NotificationId, null, null, null);

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(recoveryInfo);

            await handler.HandleAsync(new ProviderChangedEvent(NotificationId, ProvidedBy.Importer));

            A.CallTo(() => repository.Delete(recoveryInfo)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task IfRecoveryInfoDeleted_CallsSaveChanges()
        {
            var recoveryInfo = new RecoveryInfo(NotificationId, null, null, null);

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(recoveryInfo);

            await handler.HandleAsync(new ProviderChangedEvent(NotificationId, ProvidedBy.Importer));

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}
