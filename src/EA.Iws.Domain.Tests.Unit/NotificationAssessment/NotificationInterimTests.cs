namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class NotificationInterimTests
    {
        private readonly NotificationInterim notificationInterim;
        private readonly Guid notificationId = new Guid("1A714726-E219-4D04-B14C-7DB5EF751113");
        private readonly FacilityCollection facilityCollection;

        public NotificationInterimTests()
        {
            var facilityRepository = A.Fake<IFacilityRepository>();

            facilityCollection = new FacilityCollection(notificationId);

            A.CallTo(() => facilityRepository.GetByNotificationId(notificationId))
                .Returns(facilityCollection);

            notificationInterim = new NotificationInterim(facilityRepository);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CanSetValue(bool input)
        {
            await notificationInterim.SetValue(notificationId, input);

            Assert.Equal(input, facilityCollection.IsInterim);
        }

        [Fact]
        public async Task RaisesEventWhenSet()
        {
            await notificationInterim.SetValue(notificationId, true);

            Assert.NotNull(facilityCollection.Events.OfType<NotificationIsInterimSetEvent>().SingleOrDefault());
        }
    }
}