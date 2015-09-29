namespace EA.Iws.RequestHandlers.Tests.Unit.RecoveryInfo
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.RecoveryInfo;
    using Requests.RecoveryInfo;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetRecoveryInfoProviderHandlerTests
    {
        private readonly TestableNotificationApplication notification;
        private readonly INotificationApplicationRepository repository;
        private readonly GetRecoveryInfoProviderHandler handler;
        private static readonly Guid NotificationId = Guid.NewGuid();

        public GetRecoveryInfoProviderHandlerTests()
        {
            notification = new TestableNotificationApplication { Id = NotificationId };
            repository = A.Fake<INotificationApplicationRepository>();
            A.CallTo(() => repository.GetById(NotificationId)).Returns(notification);

            handler = new GetRecoveryInfoProviderHandler(repository);
        }

        [Fact]
        public async Task ReturnsNullWhenProviderNotSet()
        {
            var result = await handler.HandleAsync(new GetRecoveryInfoProvider(NotificationId));

            Assert.Null(result);
        }

        [Theory]
        [InlineData(ProvidedBy.Importer)]
        [InlineData(ProvidedBy.Notifier)]
        public async Task ReturnsCorrectProviderWhenSet(ProvidedBy providedBy)
        {
            notification.RecoveryInformationProvidedByImporter = providedBy == ProvidedBy.Importer;

            var result = await handler.HandleAsync(new GetRecoveryInfoProvider(NotificationId));

            Assert.Equal(providedBy, result);
        }
    }
}
