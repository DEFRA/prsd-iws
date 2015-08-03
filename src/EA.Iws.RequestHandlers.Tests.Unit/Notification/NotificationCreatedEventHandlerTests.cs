namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Helpers;
    using RequestHandlers.Notification;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationCreatedEventHandlerTests
    {
        private readonly NotificationCreatedEventHandler handler;
        private readonly NotificationCreatedEvent message;
        private readonly IwsContext context;
        private Guid notificationId = new Guid("00452356-08C3-4D20-A9E4-552B0FE00864");

        public NotificationCreatedEventHandlerTests()
        {
            context = A.Fake<IwsContext>();

            var dbSetHelper = new DbContextHelper();

            var assessments = dbSetHelper.GetAsyncEnabledDbSet(new List<NotificationAssessment>());

            A.CallTo(() => context.NotificationAssessments).Returns(assessments);

            var notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            message = new NotificationCreatedEvent(notification);
            handler = new NotificationCreatedEventHandler(context);
        }

        [Fact]
        public async Task NotificationAssessmentIsCreated()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.NotificationAssessments.Count(p => p.NotificationApplicationId == notificationId));
        }

        [Fact]
        public async Task NotificationAssessmentIsNotSubmitted()
        {
            await handler.HandleAsync(message);

            Assert.Equal(NotificationStatus.NotSubmitted, context.NotificationAssessments.Single(p => p.NotificationApplicationId == notificationId).Status);
        }
    }
}