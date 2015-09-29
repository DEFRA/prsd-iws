namespace EA.Iws.RequestHandlers.Tests.Unit.RecoveryInfo
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.RecoveryInfo;
    using Requests.RecoveryInfo;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetRecoveryInfoProviderHandlerTests
    {
        private static readonly Guid NotificationId = Guid.NewGuid();
        private readonly TestIwsContext context;
        private readonly INotificationApplicationRepository repository;
        private readonly SetRecoveryInfoProviderHandler handler;

        public SetRecoveryInfoProviderHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<INotificationApplicationRepository>();

            var notification = new TestableNotificationApplication
            {
                Id = NotificationId
            };

            context.NotificationApplications.Add(notification);
            A.CallTo(() => repository.GetById(NotificationId)).Returns(notification);

            handler = new SetRecoveryInfoProviderHandler(repository, context);
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            await handler.HandleAsync(new SetRecoveryInfoProvider(ProvidedBy.Importer, NotificationId));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task IsProvidedByImporter_False_WhenSetWithNotifier()
        {
            await handler.HandleAsync(new SetRecoveryInfoProvider(ProvidedBy.Notifier, NotificationId));

            var notification = context.NotificationApplications.Single(n => n.Id == NotificationId);

            Assert.False(notification.RecoveryInformationProvidedByImporter);
        }

        [Fact]
        public async Task IsProvidedByImporter_True_WhenSetWithImporter()
        {
            await handler.HandleAsync(new SetRecoveryInfoProvider(ProvidedBy.Importer, NotificationId));

            var notification = context.NotificationApplications.Single(n => n.Id == NotificationId);

            Assert.True(notification.RecoveryInformationProvidedByImporter);
        }
    }
}
