namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication.SharedUser
{
    using System.Linq;
    using Areas.NotificationApplication.Controllers;
    using Web.Controllers;
    using Web.Infrastructure;
    using Xunit;

    public class NotificationOwnerFilterControllerAttributeTests
    {
        [Fact]
        public void ChangeNotificationOwnerControllerHasNotificationOwnerFilter()
        {
            var attributes = typeof(ChangeNotificationOwnerController).GetCustomAttributes(
                typeof(NotificationOwnerFilter), true);

            Assert.True(attributes.Any());
        }

        [Fact]
        public void ShareNotificationControllerHasNotificationOwnerFilter()
        {
            var attributes = typeof(ShareNotificationController).GetCustomAttributes(
                typeof(NotificationOwnerFilter), true);

            Assert.True(attributes.Any());
        }

        [Fact]
        public void ReviewUserAccessControllerHasNotificationOwnerFilter()
        {
            var attributes = typeof(ReviewUserAccessController).GetCustomAttributes(
                typeof(NotificationOwnerFilter), true);

            Assert.True(attributes.Any());
        }
    }
}
