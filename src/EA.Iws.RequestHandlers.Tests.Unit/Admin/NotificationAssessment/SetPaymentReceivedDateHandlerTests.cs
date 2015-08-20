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

    public class SetPaymentReceivedDateHandlerTests
    {
        private readonly NotificationAssessment assessment;
        private readonly TestIwsContext context;
        private readonly SetPaymentReceivedDateHandler handler;
        private readonly SetPaymentReceivedDate message;
        private readonly Guid notificationId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly DateTime paymentReceivedDate = new DateTime(2015, 8, 1);

        public SetPaymentReceivedDateHandlerTests()
        {
            context = new TestIwsContext();
            assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived,
                assessment);

            context.NotificationAssessments.Add(assessment);

            handler = new SetPaymentReceivedDateHandler(context);
            message = new SetPaymentReceivedDate(notificationId, paymentReceivedDate);
        }

        [Fact]
        public async Task SetsPaymentReceivedDate()
        {
            await handler.HandleAsync(message);

            Assert.Equal(paymentReceivedDate,
                context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}