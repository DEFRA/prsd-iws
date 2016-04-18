namespace EA.Iws.RequestHandlers.Tests.Unit.WasteRecovery
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.WasteRecovery;
    using Requests.WasteRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetWasteRecoveryProviderHandlerTests
    {
        private readonly TestableNotificationApplication notification;
        private readonly INotificationApplicationRepository repository;
        private readonly GetWasteRecoveryProviderHandler handler;
        private static readonly Guid NotificationId = Guid.NewGuid();

        public GetWasteRecoveryProviderHandlerTests()
        {
            notification = new TestableNotificationApplication { Id = NotificationId };
            repository = A.Fake<INotificationApplicationRepository>();
            A.CallTo(() => repository.GetById(NotificationId)).Returns(notification);

            handler = new GetWasteRecoveryProviderHandler(repository);
        }

        [Fact]
        public async Task ReturnsNullWhenProviderNotSet()
        {
            var result = await handler.HandleAsync(new GetWasteRecoveryProvider(NotificationId));

            Assert.Null(result);
        }

        [Theory]
        [InlineData(ProvidedBy.Importer)]
        [InlineData(ProvidedBy.Notifier)]
        public async Task ReturnsCorrectProviderWhenSet(ProvidedBy providedBy)
        {
            notification.WasteRecoveryInformationProvidedByImporter = providedBy == ProvidedBy.Importer;

            var result = await handler.HandleAsync(new GetWasteRecoveryProvider(NotificationId));

            Assert.Equal(providedBy, result);
        }
    }
}
