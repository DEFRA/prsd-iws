namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Core.Shared;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationApplicationTests
    {
        private const string England = "England";
        private const string Scotland = "Scotland";
        private const string NorthernIreland = "Northern Ireland";
        private const string Wales = "Wales";
        private readonly NotificationApplication notification;

        public NotificationApplicationTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
        }

        [Theory]
        [InlineData("GB 0001 123456", England, 123456)]
        [InlineData("GB 0002 123456", Scotland, 123456)]
        [InlineData("GB 0003 123456", NorthernIreland, 123456)]
        [InlineData("GB 0004 123456", Wales, 123456)]
        [InlineData("GB 0001 005000", England, 5000)]
        [InlineData("GB 0002 000100", Scotland, 100)]
        public void NotificationNumberFormat(string expected, string country, int notificationNumber)
        {
            var userId = new Guid("{FCCC2E8A-2464-4C10-8521-09F16F2C550C}");
            var notificationApplication = new NotificationApplication(userId, NotificationType.Disposal,
                GetCompetentAuthority(country),
                notificationNumber);
            Assert.Equal(expected, notificationApplication.NotificationNumber);
        }

        private static UKCompetentAuthority GetCompetentAuthority(string country)
        {
            if (country == England)
            {
                return UKCompetentAuthority.England;
            }
            if (country == Scotland)
            {
                return UKCompetentAuthority.Scotland;
            }
            if (country == NorthernIreland)
            {
                return UKCompetentAuthority.NorthernIreland;
            }
            if (country == Wales)
            {
                return UKCompetentAuthority.Wales;
            }
            throw new ArgumentException("Unknown competent authority", "country");
        }

        [Fact]
        public void SetReasonForExport_ExceedMaxCharacters_ThrowsException()
        {
            const string longString = "abcdefghijklmnopqrstuvwxyxabcdefghijklmnopqrstuvwxyxabcdefghijklmnopqrs";

            Action setReasonForExport = () => notification.ReasonForExport = longString;

            Assert.Throws<InvalidOperationException>(setReasonForExport);
        }
    }
}