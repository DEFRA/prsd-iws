namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Prsd.Core;
    using RequestHandlers.Admin.NotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationStatusChangeEventHandlerTests
    {
        private const string AnyString = "test";
        private static readonly Guid UserId = new Guid("E3E92750-3BFC-4913-B152-89EC07B78CB6");
        private static readonly Guid NotificationId = new Guid("54C49AE0-D114-40DB-842D-EFBFB3B96287");
        private readonly IwsContext context;
        private readonly NotificationStatusChangeEventHandler handler;
        private readonly NotificationAssessment notificationAssessment;
        private readonly NotificationStatusChangeEvent receivedEvent;

        public NotificationStatusChangeEventHandlerTests()
        {
            context = new TestIwsContext();
            var userContext = new TestUserContext(UserId);

            handler = new NotificationStatusChangeEventHandler(context, userContext);

            context.Users.Add(UserFactory.Create(UserId, AnyString, AnyString, AnyString, AnyString));

            notificationAssessment = new NotificationAssessment(NotificationId);

            receivedEvent = new NotificationStatusChangeEvent(notificationAssessment,
                NotificationStatus.Submitted);
        }

        [Fact]
        public async Task AddsStatusChangeRecord()
        {
            var currentRecordCount = notificationAssessment.StatusChanges.Count();

            await
                handler.HandleAsync(receivedEvent);

            Assert.Equal(currentRecordCount + 1, notificationAssessment.StatusChanges.Count());
        }

        [Fact]
        public async Task AddsStatusChangeRecord_WithCorrectStatus()
        {
            Predicate<NotificationStatusChange> getSubmittedStatusChanges =
                n => n.Status == NotificationStatus.Submitted;

            Assert.DoesNotContain(notificationAssessment.StatusChanges, getSubmittedStatusChanges);

            await
                handler.HandleAsync(receivedEvent);

            Assert.Contains(notificationAssessment.StatusChanges, getSubmittedStatusChanges);
        }

        [Fact]
        public async Task AddsStatusChangeRecord_WithCorrectDate()
        {
            SystemTime.Freeze();

            var date = SystemTime.UtcNow;

            await handler.HandleAsync(receivedEvent);

            Assert.Equal(date,
                notificationAssessment.StatusChanges.Single(sc => sc.Status == NotificationStatus.Submitted).ChangeDate);

            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task SavesChanges_IsCalled()
        {
            await handler.HandleAsync(receivedEvent);

            Assert.Equal(1, ((TestIwsContext)context).SaveChangesCount);
        }

        [Fact]
        public async Task AddsStatusChangeRecord_WithCorrectUser()
        {
            await handler.HandleAsync(receivedEvent);

            Assert.Equal(UserId.ToString(),
                notificationAssessment.StatusChanges.Single(sc => sc.Status == NotificationStatus.Submitted).User.Id);
        }
    }
}