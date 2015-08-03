namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using Helpers;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetFinancialGuaranteeDatesHandlerTests
    {
        private static readonly Guid PendingNotificationId = new Guid("F6BB1BC8-3BD1-4CBE-8FAB-7CA3BE6274CD");
        private static readonly Guid ReceivedNotificationId = new Guid("53E57ABA-1891-4798-ADAB-60BB2E56A63D");
        private static readonly DateTime AnyDate = new DateTime(2014, 5, 5);

        private readonly IwsContext context;
        private readonly SetFinancialGuaranteeDatesHandler handler;

        public SetFinancialGuaranteeDatesHandlerTests()
        {
            context = A.Fake<IwsContext>();
            var dbHelper = new DbContextHelper();

            var receivedFinancialGuarantee = FinancialGuarantee.Create(ReceivedNotificationId);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.ApplicationReceived,
                receivedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate,
                receivedFinancialGuarantee);

            var pendingNotification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(pendingNotification, PendingNotificationId);
            var receivedNotification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(receivedNotification, ReceivedNotificationId);
            
            var financialGuarantees = dbHelper.GetAsyncEnabledDbSet(new[]
            {
                FinancialGuarantee.Create(PendingNotificationId),
                receivedFinancialGuarantee
            });

            var notificationApplications = dbHelper.GetAsyncEnabledDbSet(new List<NotificationApplication>()
            {
                pendingNotification,
                receivedNotification
            });

            A.CallTo(() => context.FinancialGuarantees).Returns(financialGuarantees);
            A.CallTo(() => context.NotificationApplications).Returns(notificationApplications);

            handler = new SetFinancialGuaranteeDatesHandler(context);
        }

        [Fact]
        public void Request_CannotBeInstantiated_WithNullDates()
        {
            Assert.Throws<ArgumentException>(() => new SetFinancialGuaranteeDates(Guid.Empty, null, null));
        }

        [Fact]
        public void Request_CanBeInstantiated_WithNullReceivedDate()
        {
            var result = new SetFinancialGuaranteeDates(Guid.Empty, null, AnyDate);

            Assert.Equal(AnyDate, result.CompletedDate);
        }

        [Fact]
        public void Request_CanBeInstantiated_WithNullCompletedDate()
        {
            var result = new SetFinancialGuaranteeDates(Guid.Empty, AnyDate, null);

            Assert.Equal(AnyDate, result.ReceivedDate);
        }

        [Fact]
        public async Task Handler_NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new SetFinancialGuaranteeDates(Guid.Empty, AnyDate, null)));
        }

        [Fact]
        public async Task Handler_SetReceivedDateForPendingRecord_SetsDateAndStatus()
        {
            await handler.HandleAsync(new SetFinancialGuaranteeDates(PendingNotificationId, AnyDate, null));

            var financialGuarantee =
                context.FinancialGuarantees.Single(na => na.NotificationApplicationId == PendingNotificationId);

            Assert.Equal(AnyDate, financialGuarantee.ReceivedDate);
            Assert.Equal(FinancialGuaranteeStatus.ApplicationReceived, financialGuarantee.Status);
        }

        [Fact]
        public async Task Handler_SetReceivedDatesForPendingRecord_SavesChanges()
        {
            await handler.HandleAsync(new SetFinancialGuaranteeDates(PendingNotificationId, AnyDate, null));

            A.CallTo(() => context.SaveChangesAsync()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Handler_SetCompletedDateForReceivedRecord_SetsDateAndStatus()
        {
            await
                handler.HandleAsync(new SetFinancialGuaranteeDates(ReceivedNotificationId, null,
                    AnyDate.AddDays(1)));

            var financialGuarantee =
                context.FinancialGuarantees.Single(na => na.NotificationApplicationId == ReceivedNotificationId);

            Assert.Equal(AnyDate.AddDays(1), financialGuarantee.CompletedDate);
            Assert.Equal(FinancialGuaranteeStatus.ApplicationComplete, financialGuarantee.Status);
        }
    }
}