namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using RequestHandlers.Admin.NotificationAssessment;
    using RequestHandlers.Mappings.NotificationAssessment;
    using Requests.Admin.NotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetDatesHandlerTests
    {
        private readonly TestIwsContext context;
        private readonly Guid notificationId1 = new Guid("91E55968-E01A-4015-891D-B613820B50AB");
        private readonly DateTime receivedDate = new DateTime(2015, 8, 1);
        private readonly Guid notificationId2 = new Guid("35D538A0-1FD9-4C4A-AB75-A9F81CE5E67A");
        private readonly GetDatesHandler handler;
        private readonly DateTime paymentDate = new DateTime(2015, 8, 2);
        private readonly DateTime commencementDate = new DateTime(2015, 8, 10);
        private readonly DateTime completedDate = new DateTime(2015, 8, 20);
        private string nameOfOfficer = "officer";
        private DateTime transmitDate = new DateTime(2015, 8, 22);

        public GetDatesHandlerTests()
        {
            context = new TestIwsContext();

            var assessment1 = new NotificationAssessment(notificationId1);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.NotificationReceivedDate, receivedDate, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, paymentDate, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.CommencementDate, commencementDate, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.NameOfOfficer, nameOfOfficer, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.CompleteDate, completedDate, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.TransmittedDate, transmitDate, assessment1.Dates);

            var assessment2 = new NotificationAssessment(notificationId2);

            context.NotificationAssessments.AddRange(new[]
            {
                assessment1,
                assessment2
            });

            handler = new GetDatesHandler(context, new NotificationDatesMap());
        }

        [Fact]
        public async Task SetsNotificationId()
        {
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(notificationId1, result.NotificationId);
        }

        [Fact]
        public async Task HasReceivedDate_SetsReceivedDate()
        {
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(receivedDate, result.NotificationReceivedDate);
        }

        [Fact]
        public async Task HasNoReceivedDate_ReceivedDateIsNull()
        {
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.NotificationReceivedDate);
        }

        [Fact]
        public async Task HasPaymentDate_SetsPaymentDate()
        {
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(paymentDate, result.PaymentReceivedDate);
        }

        [Fact]
        public async Task HasNoPaymentDate_PaymentDateIsNull()
        {
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.PaymentReceivedDate);
        }

        [Fact]
        public async Task HasCommencementDate_SetsCommencementDate()
        {
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(commencementDate, result.CommencementDate);
        }

        [Fact]
        public async Task HasNoCommencementDate_CommencementDateIsNull()
        {
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.CommencementDate);
        }

        [Fact]
        public async Task HasCommencementDate_SetsNameOfOfficer()
        {
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(nameOfOfficer, result.NameOfOfficer);
        }

        [Fact]
        public async Task HasNoCommencementDate_NameOfOfficerIsNull()
        {
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.NameOfOfficer);
        }

        [Fact]
        public async Task HasCompletedDate_SetsCompletedDate()
        {
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(completedDate, result.CompletedDate);
        }

        [Fact]
        public async Task HasNoCompletedDate_CompletedDateIsNull()
        {
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.CompletedDate);
        }

        [Fact]
        public async Task HasTransmitDate_SetsTransmitDate()
        {
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(transmitDate, result.TransmittedDate);
        }

        [Fact]
        public async Task HasNoTransmitDate_TransmitDateIsNull()
        {
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.TransmittedDate);
        }
    }
}