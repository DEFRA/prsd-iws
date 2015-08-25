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

    public class SetNotificationTransmittedDateHandlerTests
    {
        private readonly NotificationAssessment assessment;
        private readonly TestIwsContext context;
        private readonly SetNotificationTransmittedDateHandler handler;
        private readonly SetNotificationTransmittedDate message;
        private readonly Guid notificationId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly DateTime transmittedDate = new DateTime(2015, 8, 1);

        public SetNotificationTransmittedDateHandlerTests()
        {
            context = new TestIwsContext();
            assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.ReadyToTransmit,
                assessment);

            context.NotificationAssessments.Add(assessment);

            handler = new SetNotificationTransmittedDateHandler(context);
            message = new SetNotificationTransmittedDate(notificationId, transmittedDate);
        }

        [Fact]
        public async Task SetsNotificationTransmittedDate()
        {
            await handler.HandleAsync(message);

            Assert.Equal(transmittedDate,
                context.NotificationAssessments.Single().Dates.TransmittedDate);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}