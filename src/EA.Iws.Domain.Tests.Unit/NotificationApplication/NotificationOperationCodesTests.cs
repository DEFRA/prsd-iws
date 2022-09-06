namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Xunit;

    public class NotificationOperationCodesTests
    {
        [Fact]
        public void CanAddRecoveryCodesToRecoveryNotification()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var codes = new List<OperationCode>
            {
                OperationCode.R1,
                OperationCode.R2
            };

            notification.SetOperationCodes(codes);

            Assert.Collection(notification.OperationInfos,
                item => Assert.Equal(OperationCode.R1, notification.OperationInfos.ElementAt(0).OperationCode),
                item => Assert.Equal(OperationCode.R2, notification.OperationInfos.ElementAt(1).OperationCode));
        }

        [Fact]
        public void CanAddDisposalCodesToDisposalNotification()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal,
                UKCompetentAuthority.England, 0);

            var codes = new List<OperationCode>
            {
                OperationCode.D1,
                OperationCode.D2
            };

            notification.SetOperationCodes(codes);

            Assert.Collection(notification.OperationInfos,
                item => Assert.Equal(OperationCode.D1, notification.OperationInfos.ElementAt(0).OperationCode),
                item => Assert.Equal(OperationCode.D2, notification.OperationInfos.ElementAt(1).OperationCode));
        }

        [Fact]
        public void CantAddDisposalCodesToRecoveryNotification()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var codes = new List<OperationCode>
            {
                OperationCode.D1,
                OperationCode.D2
            };

            Action updateCodes = () => notification.SetOperationCodes(codes);

            Assert.Throws<InvalidOperationException>(updateCodes);
        }

        [Fact]
        public void CantAddRecoveryCodesToDisposalNotification()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal,
                UKCompetentAuthority.England, 0);

            var codes = new List<OperationCode>
            {
                OperationCode.R1,
                OperationCode.R2
            };

            Action updateCodes = () => notification.SetOperationCodes(codes);

            Assert.Throws<InvalidOperationException>(updateCodes);
        }

        [Fact]
        public void UpdateCodesReplacesItems()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var codes = new List<OperationCode>
            {
                OperationCode.R1,
                OperationCode.R2
            };

            var newCodes = new List<OperationCode>
            {
                OperationCode.R3,
                OperationCode.R4
            };

            notification.SetOperationCodes(codes);
            notification.SetOperationCodes(newCodes);

            Assert.Collection(notification.OperationInfos,
                item => Assert.Equal(OperationCode.R3, notification.OperationInfos.ElementAt(0).OperationCode),
                item => Assert.Equal(OperationCode.R4, notification.OperationInfos.ElementAt(1).OperationCode));
        }
    }
}