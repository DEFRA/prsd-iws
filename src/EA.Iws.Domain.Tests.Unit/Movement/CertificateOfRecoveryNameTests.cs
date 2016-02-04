namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Xunit;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class CertificateOfRecoveryNameTests
    {
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private readonly CertificateOfRecoveryNameGenerator certificateOfRecoveryName;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly Guid notificationId;

        public CertificateOfRecoveryNameTests()
        {
            notificationRepository = A.Fake<INotificationApplicationRepository>();
            notificationId = new Guid("231208B0-7341-415E-9396-014AEBB0D3E1");
            certificateOfRecoveryName = new CertificateOfRecoveryNameGenerator(notificationRepository);
        }

        [Theory]
        [MemberData("TestData")]
        public async Task ReturnsCorrectFormat(string expected, CompetentAuthorityEnum competentAuthority, int notificationNumber, NotificationType notificationType, int movementNumber)
        {
            var notification = new NotificationApplication(Guid.NewGuid(), notificationType, competentAuthority, notificationNumber);
            A.CallTo(() => notificationRepository.GetById(notificationId)).Returns(notification);

            var result = await certificateOfRecoveryName.GetValue(new Movement(movementNumber, notificationId, AnyDate));

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> TestData()
        {
            return new[]
            {
                new object[] { "GB0001000123-shipment-1-recovery-receipt", CompetentAuthorityEnum.England, 123, NotificationType.Recovery, 1 },
                new object[] { "GB0002005555-shipment-7-disposal-receipt", CompetentAuthorityEnum.Scotland, 5555, NotificationType.Disposal, 7 },
                new object[] { "GB0003999999-shipment-600-recovery-receipt", CompetentAuthorityEnum.NorthernIreland, 999999, NotificationType.Recovery, 600 },
                new object[] { "GB0004000003-shipment-9000-disposal-receipt", CompetentAuthorityEnum.Wales, 3, NotificationType.Disposal, 9000 }
            };
        }
    }
}