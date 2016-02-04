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

    public class CertificateOfReceiptNameTests
    {
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private readonly CertificateOfReceiptNameGenerator certificateOfReceiptName;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly Guid notificationId;

        public CertificateOfReceiptNameTests()
        {
            notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();
            notificationId = new Guid("6CE221A8-F8EC-4EDD-A2D7-33B8836B9BBF");

            certificateOfReceiptName = new CertificateOfReceiptNameGenerator(notificationApplicationRepository);
        }

        [Theory]
        [MemberData("TestData")]
        public async Task ReturnsCorrectFormat(CompetentAuthorityEnum competentAuthority, int notificationNumber, int movementNumber, string expected)
        {
            var notification = new NotificationApplication(notificationId, NotificationType.Recovery, competentAuthority, notificationNumber);
            A.CallTo(() => notificationApplicationRepository.GetById(notificationId)).Returns(notification);

            var movement = new Movement(movementNumber, notificationId, AnyDate);
            var name = await certificateOfReceiptName.GetValue(movement);

            Assert.Equal(expected, name);
        }

        public static IEnumerable<object[]> TestData()
        {
            return new[]
            {
                new object[] { CompetentAuthorityEnum.England, 9999, 1, "GB0001009999-shipment-1-receipt" },
                new object[] { CompetentAuthorityEnum.Scotland, 444, 2, "GB0002000444-shipment-2-receipt" },
                new object[] { CompetentAuthorityEnum.NorthernIreland, 5, 6, "GB0003000005-shipment-6-receipt" },
                new object[] { CompetentAuthorityEnum.Wales, 222222, 4, "GB0004222222-shipment-4-receipt" }
            };
        } 
    }
}