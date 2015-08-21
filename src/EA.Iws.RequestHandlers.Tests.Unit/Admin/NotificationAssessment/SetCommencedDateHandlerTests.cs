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

    public class SetCommencedDateHandlerTests
    {
        private Guid notificationId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly TestIwsContext context;
        private readonly DateTime commencementDate = new DateTime(2015, 8, 10);
        private readonly SetCommencedDateHandler handler;
        private readonly SetCommencedDate message;

        public SetCommencedDateHandlerTests()
        {
            context = new TestIwsContext();
            var assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.Submitted, assessment);

            context.NotificationAssessments.Add(assessment);

            message = new SetCommencedDate(notificationId, commencementDate, "Officer");
            handler = new SetCommencedDateHandler(context);
        }

        [Fact]
        public async Task SetsCommencementDate()
        {
            await handler.HandleAsync(message);

            Assert.Equal(commencementDate,
                context.NotificationAssessments.Single().Dates.CommencementDate);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task SetsNameOfOfficer()
        {
            await handler.HandleAsync(message);

            Assert.Equal("Officer", context.NotificationAssessments.Single().Dates.NameOfOfficer);
        }
    }
}