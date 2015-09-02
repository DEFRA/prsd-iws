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

    public class SetNotificationAcknowledgedDateHandlerTests
    {
        private readonly SetNotificationAcknowledgedDateHandler handler;
        private readonly DateTime notificationAcknowledgedDate = new DateTime(2015, 8, 1);
        private readonly Guid notificationId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly NotificationAssessment assessment;
        private readonly TestIwsContext context;
        private readonly SetNotificationAcknowledgedDate message;

        public SetNotificationAcknowledgedDateHandlerTests()
        {
            context = new TestIwsContext();
            assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.Transmitted, assessment);

            context.NotificationAssessments.Add(assessment);

            handler = new SetNotificationAcknowledgedDateHandler(context);
            message = new SetNotificationAcknowledgedDate(notificationId, notificationAcknowledgedDate);
        }

        [Fact]
        public async Task SetsNotificationAcknowledgedDate()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notificationAcknowledgedDate,
                context.NotificationAssessments.Single().Dates.AcknowledgedDate);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}