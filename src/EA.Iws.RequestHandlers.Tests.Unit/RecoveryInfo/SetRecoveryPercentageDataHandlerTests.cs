namespace EA.Iws.RequestHandlers.Tests.Unit.RecoveryInfo
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Helpers;
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
            context = A.Fake<IwsContext>();
            var helper = new DbContextHelper();
            notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            A.CallTo(() => context.NotificationApplications).Returns(helper.GetAsyncEnabledDbSet(new[]
            {
                notification
            }));

            setHandler = new SetRecoveryPercentageDataHandler(context);
        }

        [Fact]
        public async Task SetRecoveryData_When_InfoProvidedByImporter_CheckBoxIsChecked()
        {
            var request = new SetRecoveryPercentageData(notificationId, checkboxChecked, "anyMethodOfDisposal", anyPercentageRecoverable);
            await setHandler.HandleAsync(request);

            Assert.True(notification.IsProvidedByImporter);
            Assert.Null(notification.MethodOfDisposal);
            Assert.Null(notification.PercentageRecoverable);
        }

        [Fact]
        public async Task SetRecoveryPercentage_And_MethodOfDisposal_When_ValidPercentageValue()
        {
            await SavesSuccessfullyWithCheckboxUncheckedAnd(99.99M, "valid MethodOfDisposal");
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
            var request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, "anyMethodOfDisposal", hundredPercentage);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await setHandler.HandleAsync(request));
        }

        [Fact]
        public async Task SetRecovery_Should_Return_Provided_NotificationId()
        {
            var request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, "anyMethodOfDisposal", 10M);
            var response = await setHandler.HandleAsync(request);

            Assert.Equal(notificationId, response);
        }

        private async Task SavesSuccessfullyWithCheckboxUncheckedAnd(decimal? percentage, string methodOfDisposal)
        {
            var request = new SetRecoveryPercentageData(notificationId, !checkboxChecked, methodOfDisposal, percentage);
            await setHandler.HandleAsync(request);

            Assert.Null(notification.IsProvidedByImporter);
            Assert.Equal(percentage, notification.PercentageRecoverable.GetValueOrDefault());
            Assert.Equal(methodOfDisposal, notification.MethodOfDisposal);
        }
    }
}
