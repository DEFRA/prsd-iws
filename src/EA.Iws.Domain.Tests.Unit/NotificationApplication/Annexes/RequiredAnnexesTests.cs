namespace EA.Iws.Domain.Tests.Unit.NotificationApplication.Annexes
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Annexes;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class RequiredAnnexesTests
    {
        private readonly TestableNotificationApplication notification;

        private readonly RequiredAnnexes requiredAnnexes;

        public RequiredAnnexesTests()
        {
            var notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();

            notification = new TestableNotificationApplication();

            A.CallTo(() => notificationApplicationRepository.GetById(A<Guid>.Ignored)).Returns(notification);

            requiredAnnexes = new RequiredAnnexes(notificationApplicationRepository);
        }

        [Fact]
        public async Task NoAnnexesRequired()
        {
            notification.TechnologyEmployed = new TestableTechnologyEmployed
            {
                AnnexProvided = false
            };

            var result = await requiredAnnexes.Get(Guid.Empty);

            Assert.False(result.IsProcessOfGenerationRequired);
            Assert.False(result.IsTechnologyEmployedRequired);
            Assert.False(result.IsWasteCompositionRequired);
        }

        [Fact]
        public async Task TechnologyEmployedRequired()
        {
            notification.TechnologyEmployed = new TestableTechnologyEmployed
            {
                AnnexProvided = true
            };

            var result = await requiredAnnexes.Get(Guid.Empty);

            Assert.True(result.IsTechnologyEmployedRequired);
        }

        [Fact]
        public async Task WasteCompositonRequired()
        {
            notification.WasteType = new TestableWasteType
            {
                HasAnnex = true
            };

            var result = await requiredAnnexes.Get(Guid.Empty);

            Assert.True(result.IsWasteCompositionRequired);
        }

        [Fact]
        public async Task ProcessOfGenerationRequired()
        {
            notification.IsWasteGenerationProcessAttached = true;
            
            var result = await requiredAnnexes.Get(Guid.Empty);

            Assert.True(result.IsProcessOfGenerationRequired);
        } 
    }
}
