namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using RequestHandlers.Admin.NotificationAssessment;
    using Requests.Admin.NotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetNotificationCompleteDateHandlerTests
    {
        private readonly SetNotificationCompleteDate message;
        private readonly SetNotificationCompleteDateHandler handler;
        private readonly TestIwsContext context;
        private readonly DateTime notificationCompleteDate = new DateTime(2015, 8, 20);
        private readonly Guid notificationId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly NotificationAssessment assessment;

        public SetNotificationCompleteDateHandlerTests()
        {
            context = new TestIwsContext();
            assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.InAssessment, assessment);

            context.NotificationAssessments.Add(assessment);

            message = new SetNotificationCompleteDate(notificationId, notificationCompleteDate);
            handler = new SetNotificationCompleteDateHandler(context);
        }

        [Fact]
        public async Task SetsNotificationCompleteDate()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notificationCompleteDate,
                context.NotificationAssessments.Single().Dates.CompleteDate);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}