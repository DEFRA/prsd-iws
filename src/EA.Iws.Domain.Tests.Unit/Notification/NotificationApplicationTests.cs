namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using Xunit;

    public class NotificationApplicationTests
    {
        private const string England = "England";
        private const string Scotland = "Scotland";
        private const string NorthernIreland = "Northern Ireland";
        private const string Wales = "Wales";

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
            var notification = new NotificationApplication(userId, NotificationType.Disposal, GetCompetentAuthority(country),
                notificationNumber);
            Assert.Equal(expected, notification.NotificationNumber);
        }

        private UKCompetentAuthority GetCompetentAuthority(string country)
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
        public void ProducersCanOnlyHaveOneSiteOfExport()
        {
            var address = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                "United Kingdom");

            var business = new Business(string.Empty, String.Empty, String.Empty, string.Empty);

            var contact = new Contact(string.Empty, String.Empty, String.Empty, String.Empty);

            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            notification.AddProducer(new Producer(business, address, contact, true));
            notification.AddProducer(new Producer(business, address, contact, true));

            var siteOfExportCount = notification.Producers.Count(p => p.IsSiteOfExport);
            Assert.Equal(1, siteOfExportCount);
        }
    }
}