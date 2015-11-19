namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using RequestHandlers.Admin.NotificationAssessment;
    using RequestHandlers.Mappings.NotificationAssessment;
    using Requests.Admin.NotificationAssessment;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetDatesHandlerTests
    {
        private readonly TestIwsContext context = new TestIwsContext();

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
            var decisionRequiredBy = A.Fake<DecisionRequiredBy>();
            var notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();
            var notificationAssessmentRepository = A.Fake<INotificationAssessmentRepository>();
            var transactionRepository = A.Fake<INotificationTransactionRepository>();
            var transactionCalculator = A.Fake<NotificationTransactionCalculator>();
            var chargeCalculator = A.Fake<INotificationChargeCalculator>();

            var assessment1 = new NotificationAssessment(notificationId1);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.NotificationReceivedDate, receivedDate, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, paymentDate, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.CommencementDate, commencementDate, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.NameOfOfficer, nameOfOfficer, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.CompleteDate, completedDate, assessment1.Dates);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.TransmittedDate, transmitDate, assessment1.Dates);

            var assessment2 = new NotificationAssessment(notificationId2);

            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(notificationId1)).Returns(assessment1);
            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(notificationId2)).Returns(assessment2);

            A.CallTo(() => notificationApplicationRepository.GetById(notificationId1))
                .Returns(new TestableNotificationApplication
                {
                    Id = notificationId1
                });

            A.CallTo(() => notificationApplicationRepository.GetById(notificationId2))
                .Returns(new TestableNotificationApplication
                {
                    Id = notificationId2
                });
            
            handler = new GetDatesHandler(context, 
                new NotificationDatesMap(), 
                decisionRequiredBy, 
                notificationAssessmentRepository, 
                notificationApplicationRepository,
                transactionRepository,
                transactionCalculator,
                chargeCalculator);
            
            A.CallTo(() => chargeCalculator.GetValue(A<PricingStructure[]>.Ignored, A<NotificationApplication>.Ignored, A<ShipmentInfo>.Ignored)).Returns(200.00m);
        }

        [Fact]
        public async Task SetsNotificationId()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId1 });
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(notificationId1, result.NotificationId);
        }

        [Fact]
        public async Task HasReceivedDate_SetsReceivedDate()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId1 });
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(receivedDate, result.NotificationReceivedDate);
        }

        [Fact]
        public async Task HasNoReceivedDate_ReceivedDateIsNull()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId2 });
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.NotificationReceivedDate);
        }

        [Fact]
        public async Task HasNoPaymentDate_PaymentDateIsNull()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId2 });
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.PaymentReceivedDate);
        }

        [Fact]
        public async Task HasCommencementDate_SetsCommencementDate()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId1 });
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(commencementDate, result.CommencementDate);
        }

        [Fact]
        public async Task HasNoCommencementDate_CommencementDateIsNull()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId2 });
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.CommencementDate);
        }

        [Fact]
        public async Task HasCommencementDate_SetsNameOfOfficer()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId1 });
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(nameOfOfficer, result.NameOfOfficer);
        }

        [Fact]
        public async Task HasNoCommencementDate_NameOfOfficerIsNull()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId2 });
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.NameOfOfficer);
        }

        [Fact]
        public async Task HasCompletedDate_SetsCompletedDate()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId1 });
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(completedDate, result.CompletedDate);
        }

        [Fact]
        public async Task HasNoCompletedDate_CompletedDateIsNull()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId2 });
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.CompletedDate);
        }

        [Fact]
        public async Task HasTransmitDate_SetsTransmitDate()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId1 });
            var message = new GetDates(notificationId1);

            var result = await handler.HandleAsync(message);

            Assert.Equal(transmitDate, result.TransmittedDate);
        }

        [Fact]
        public async Task HasNoTransmitDate_TransmitDateIsNull()
        {
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = notificationId2 });
            var message = new GetDates(notificationId2);

            var result = await handler.HandleAsync(message);

            Assert.Null(result.TransmittedDate);
        }
    }
}