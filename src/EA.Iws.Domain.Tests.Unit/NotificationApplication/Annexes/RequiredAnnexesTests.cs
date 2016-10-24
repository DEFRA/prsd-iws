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
        private TechnologyEmployed technologyEmployed;
        private readonly Guid notificationId = new Guid("F1105BF7-5119-43AA-8157-444F605332E6");
        private ITechnologyEmployedRepository technologyEmployedRepository;

        public RequiredAnnexesTests()
        {
            var notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();
            technologyEmployedRepository = A.Fake<ITechnologyEmployedRepository>();

            notification = new TestableNotificationApplication();
            notification.Id = notificationId;
            technologyEmployed = TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(notificationId, "details", "further details");

            A.CallTo(() => notificationApplicationRepository.GetById(notificationId)).Returns(notification);

            A.CallTo(() => technologyEmployedRepository.GetByNotificaitonId(notificationId))
                .Returns(technologyEmployed);

            requiredAnnexes = new RequiredAnnexes(notificationApplicationRepository, technologyEmployedRepository);
        }

        [Fact]
        public async Task NoAnnexesRequired()
        {
            var result = await requiredAnnexes.Get(notificationId);

            Assert.False(result.IsProcessOfGenerationRequired);
            Assert.False(result.IsTechnologyEmployedRequired);
            Assert.False(result.IsWasteCompositionRequired);
        }

        [Fact]
        public async Task TechnologyEmployedRequired()
        {
            technologyEmployed = new TestableTechnologyEmployed
            {
                AnnexProvided = true
            };

            A.CallTo(() => technologyEmployedRepository.GetByNotificaitonId(notificationId))
                .Returns(technologyEmployed);

            var result = await requiredAnnexes.Get(notificationId);

            Assert.True(result.IsTechnologyEmployedRequired);
        }

        [Fact]
        public async Task WasteCompositonRequired()
        {
            notification.WasteType = new TestableWasteType
            {
                HasAnnex = true
            };

            var result = await requiredAnnexes.Get(notificationId);

            Assert.True(result.IsWasteCompositionRequired);
        }

        [Fact]
        public async Task ProcessOfGenerationRequired()
        {
            notification.IsWasteGenerationProcessAttached = true;
            
            var result = await requiredAnnexes.Get(notificationId);

            Assert.True(result.IsProcessOfGenerationRequired);
        } 
    }
}
