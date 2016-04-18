namespace EA.Iws.RequestHandlers.Tests.Unit.WasteRecovery
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.WasteRecovery;
    using Requests.WasteRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetWasteRecoveryProviderHandlerTests
    {
        private static readonly Guid NotificationId = Guid.NewGuid();
        private readonly TestIwsContext context;
        private readonly INotificationApplicationRepository repository;
        private readonly SetWasteRecoveryProviderHandler handler;

        public SetWasteRecoveryProviderHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<INotificationApplicationRepository>();

            var notification = new TestableNotificationApplication
            {
                Id = NotificationId
            };

            context.NotificationApplications.Add(notification);
            A.CallTo(() => repository.GetById(NotificationId)).Returns(notification);

            handler = new SetWasteRecoveryProviderHandler(repository, context);
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            await handler.HandleAsync(new SetWasteRecoveryProvider(ProvidedBy.Importer, NotificationId));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task IsProvidedByImporter_False_WhenSetWithNotifier()
        {
            await handler.HandleAsync(new SetWasteRecoveryProvider(ProvidedBy.Notifier, NotificationId));

            var notification = context.NotificationApplications.Single(n => n.Id == NotificationId);

            Assert.False(notification.WasteRecoveryInformationProvidedByImporter);
        }

        [Fact]
        public async Task IsProvidedByImporter_True_WhenSetWithImporter()
        {
            await handler.HandleAsync(new SetWasteRecoveryProvider(ProvidedBy.Importer, NotificationId));

            var notification = context.NotificationApplications.Single(n => n.Id == NotificationId);

            Assert.True(notification.WasteRecoveryInformationProvidedByImporter);
        }
    }
}
