namespace EA.Iws.Web.Tests.Unit.ViewModels.Admin
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Areas.Admin.ViewModels.ImportNotification;
    using TestHelpers;
    using Xunit;

    public class NotificationNumberViewModelTests
    {
        private const string AnyNotificationNumber = "AnyNotificationNumber";
        private readonly NotificationNumberViewModel model;

        public NotificationNumberViewModelTests()
        {
            model = new NotificationNumberViewModel
            {
                NotificationNumber = AnyNotificationNumber
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void NotificationNumberIsNullOrEmptyOrWhitespace_ReturnsError(string notificationNumber)
        {
            model.NotificationNumber = notificationNumber;
            List<ValidationResult> result = ViewModelValidator.ValidateViewModel(model);
            Assert.Equal(1, result.Count);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("GB0001005000", "GB0001005000")]
        [InlineData("GB 0001 005000", "GB0001005000")]
        [InlineData("   GB 0001 005000  ", "GB0001005000")]
        public void NotificationNumberWithOrWithoutSpace_ReturnsValueWithoutSpace(string notificationNumber, string expectedNotificationNumber)
        {
            model.NotificationNumber = notificationNumber;
            Assert.Equal(model.NotificationNumber, expectedNotificationNumber);
        }
    }
}
