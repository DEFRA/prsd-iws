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

    public class SetNotificationReceivedDateHandlerTests
    {
        private readonly SetNotificationReceivedDateHandler handler;
        private readonly DateTime notificationReceivedDate = new DateTime(2015, 8, 1);
        private readonly Guid notificationId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly NotificationAssessment assessment;
        private readonly TestIwsContext context;
        private readonly SetNotificationReceivedDate message;

        public SetNotificationReceivedDateHandlerTests()
        {
            context = new TestIwsContext();
            assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.Submitted, assessment);

            context.NotificationAssessments.Add(assessment);

            handler = new SetNotificationReceivedDateHandler(context);
            message = new SetNotificationReceivedDate(notificationId, notificationReceivedDate);
        }

        [Fact]
        public async Task SetsNotificationReceivedDate()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notificationReceivedDate,
                context.NotificationAssessments.Single().Dates.NotificationReceivedDate);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}