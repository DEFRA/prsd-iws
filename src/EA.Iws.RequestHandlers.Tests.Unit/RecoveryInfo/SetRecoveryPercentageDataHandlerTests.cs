namespace EA.Iws.RequestHandlers.Tests.Unit.RecoveryInfo
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using RequestHandlers.Notification;
    using Requests.Notification;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetRecoveryPercentageDataHandlerTests
    {
        private readonly IwsContext context;
        private readonly SetRecoveryPercentageDataHandler setHandler;
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly NotificationApplication notification;
        private readonly decimal? anyPercentageRecoverable = 1.2M;
        private readonly bool checkboxChecked = true;
        private readonly string noMethodOfDisposal = null;

        public SetRecoveryPercentageDataHandlerTests()
        {
            context = new TestIwsContext();
            notification = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            context.NotificationApplications.Add(notification);

            setHandler = new SetRecoveryPercentageDataHandler(context);
        }

        [Fact]
        public async Task SetRecoveryData_When_InfoProvidedByImporter_CheckBoxIsChecked()
        {
            var request = new SetRecoveryPercentageData(notificationId, checkboxChecked, anyPercentageRecoverable, "anyMethodOfDisposal");
            await setHandler.HandleAsync(request);

            Assert.True(notification.IsProvidedByImporter);
            Assert.Null(notification.MethodOfDisposal);
            Assert.Null(notification.PercentageRecoverable);
        }

        [Fact]
        public async Task SetMethodOfDisposal_When_ValidPercentageValue()
        {
            var validMethodOfDisposal = "Valid Method Of Disposal";
            var request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, 99.99M, null);
            await setHandler.HandleAsync(request);
            request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, 99.99M, validMethodOfDisposal);
            await setHandler.HandleAsync(request);
            Assert.Equal(validMethodOfDisposal, notification.MethodOfDisposal);
        }

        [Fact]
        public async Task SetRecovery_When_PercentageIsHundred_And_NoMethodOfDisposal_Saves_Successfully()
        {
            await SavesSuccessfullyWithCheckboxUncheckedAnd(100M, noMethodOfDisposal);
        }

        [Fact]
        public async Task SetRecovery_When_PercentageIsHundred_CanNotSet_MethodOfDisposal()
        {
            var hundredPercentage = 100M;
            var request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, 100M, null);
            await setHandler.HandleAsync(request);
            request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, hundredPercentage, "anyMethodOfDisposal");
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await setHandler.HandleAsync(request));
        }

        [Fact]
        public async Task SetRecovery_Should_Return_Provided_NotificationId()
        {
            var request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, 10M, "anyMethodOfDisposal");
            var response = await setHandler.HandleAsync(request);

            Assert.Equal(notificationId, response);
        }

        private async Task SavesSuccessfullyWithCheckboxUncheckedAnd(decimal? percentage, string methodOfDisposal)
        {
            var request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, percentage, methodOfDisposal);
            await setHandler.HandleAsync(request);

            Assert.Null(notification.IsProvidedByImporter);
            Assert.Equal(percentage, notification.PercentageRecoverable.GetValueOrDefault());
            Assert.Equal(methodOfDisposal, notification.MethodOfDisposal);
        }
    }
}
