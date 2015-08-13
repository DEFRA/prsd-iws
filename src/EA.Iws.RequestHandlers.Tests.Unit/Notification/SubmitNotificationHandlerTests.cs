namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using RequestHandlers.Notification;
    using Requests.Notification;
    using Xunit;

    public class SubmitNotificationHandlerTests
    {
        private readonly SubmitNotificationHandler handler;
        private readonly Guid notificationId = new Guid("8D41E751-D4F3-4182-A6FD-B8FE4EE897D1");
        private readonly SubmitNotification message;
        private readonly IwsContext context;
        private readonly INotificationProgressService progressService;

        public SubmitNotificationHandlerTests()
        {
            context = new TestIwsContext();
            progressService = A.Fake<INotificationProgressService>();

            var assessments = new List<NotificationAssessment>
            {
                new NotificationAssessment(notificationId)
            };

            context.NotificationAssessments.AddRange(assessments);
            A.CallTo(() => progressService.IsComplete(notificationId)).Returns(true);

            handler = new SubmitNotificationHandler(context, progressService);
            message = new SubmitNotification(notificationId);
        }

        [Fact]
        public async Task NotificationIsSubmitted()
        {
            await handler.HandleAsync(message);

            Assert.Equal(NotificationStatus.Submitted, context.NotificationAssessments.Single().Status);
        }
    }
}