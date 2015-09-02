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

    public class SetNotificationDecisionRequiredByDateHandlerTests
    {
        private readonly SetNotificationDecisionRequiredByDateHandler handler;
        private readonly DateTime notificationDecisionRequiredByDate = new DateTime(2015, 8, 1);
        private readonly Guid notificationId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly NotificationAssessment assessment;
        private readonly TestIwsContext context;
        private readonly SetNotificationDecisionRequiredByDate message;

        public SetNotificationDecisionRequiredByDateHandlerTests()
        {
            context = new TestIwsContext();
            assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.Acknowledged, assessment);

            context.NotificationAssessments.Add(assessment);

            handler = new SetNotificationDecisionRequiredByDateHandler(context);
            message = new SetNotificationDecisionRequiredByDate(notificationId, notificationDecisionRequiredByDate);
        }

        [Fact]
        public async Task SetsNotificationDecisionRequiredByDate()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notificationDecisionRequiredByDate,
                context.NotificationAssessments.Single().Dates.DecisionDate);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}