namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;

    public class NotificationApplicationFactoryTests
    {
        private readonly NotificationApplicationFactory factory;
        private readonly INotificationNumberGenerator numberGenerator;
        private readonly IUserContext userContext;

        public NotificationApplicationFactoryTests()
        {
            userContext = A.Fake<IUserContext>();
            numberGenerator = A.Fake<INotificationNumberGenerator>();

            A.CallTo(() => userContext.UserId).Returns(new Guid("246D5402-C835-4448-AFF0-37940B9ED436"));
            A.CallTo(() => numberGenerator.GetNextNotificationNumber(UKCompetentAuthority.England))
                .Returns(5000);

            factory = new NotificationApplicationFactory(userContext, numberGenerator);
        }

        [Fact]
        public async Task CanCreateNew()
        {
            var notification = await factory.CreateNew(NotificationType.Recovery, UKCompetentAuthority.England);

            Assert.IsType<NotificationApplication>(notification);
        }

        [Fact]
        public async Task CanCreateLegacy()
        {
            var notification = await factory.CreateLegacy(NotificationType.Recovery, UKCompetentAuthority.England, 100);

            Assert.IsType<NotificationApplication>(notification);
        }

        [Theory]
        [MemberData("NumbersGreaterThanOrEqualToSystemStart")]
        public async Task CantEnterNumbersGreaterThanOrEqualToSystemStart(NotificationType notificationType,
            UKCompetentAuthority competentAuthority, int number)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>("number",
                () => factory.CreateLegacy(notificationType, competentAuthority, number));
        }

        [Theory]
        [MemberData("NumbersLessThanSystemStart")]
        public async Task CanOnlyEnterNumbersLessThanSystemStart(NotificationType notificationType,
            UKCompetentAuthority competentAuthority, int number)
        {
            var notification = await factory.CreateLegacy(notificationType, competentAuthority, number);

            Assert.IsType<NotificationApplication>(notification);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task NumberCantBeZeroOrNegative(int number)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>("number",
                () => factory.CreateLegacy(NotificationType.Recovery, UKCompetentAuthority.England, number));
        }

        public static IEnumerable<object[]> NumbersGreaterThanOrEqualToSystemStart()
        {
            return new[]
            {
                new object[] { NotificationType.Recovery, UKCompetentAuthority.England, 5000 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.England, 5001 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.Scotland, 500 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.Scotland, 501 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.NorthernIreland, 1000 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.NorthernIreland, 1001 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.Wales, 100 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.Wales, 101 }
            };
        }

        public static IEnumerable<object[]> NumbersLessThanSystemStart()
        {
            return new[]
            {
                new object[] { NotificationType.Recovery, UKCompetentAuthority.England, 100 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.Scotland, 50 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.NorthernIreland, 200 },
                new object[] { NotificationType.Recovery, UKCompetentAuthority.Wales, 20 }
            };
        }
    }
}