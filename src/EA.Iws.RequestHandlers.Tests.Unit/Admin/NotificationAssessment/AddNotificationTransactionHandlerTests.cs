namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using RequestHandlers.NotificationAssessment;
    using Requests.NotificationAssessment;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class AddNotificationTransactionHandlerTests
    {
        private const decimal TotalBillable = 200.00m;
        private static readonly DateTime NewDate = new DateTime(2015, 11, 01);
        private readonly DateTime existingDate = new DateTime(2015, 10, 01);
        private readonly DateTime newerDate = new DateTime(2015, 11, 02);

        private static readonly Guid NotificationId = new Guid("8B557FD5-B2DB-4FE9-B80E-D4DA629B2B51");

        private readonly AddNotificationTransactionHandler handler;
        private readonly TestIwsContext context = new TestIwsContext();
        private readonly NotificationAssessment assessment;
        private readonly List<NotificationTransaction> transactions;

        public AddNotificationTransactionHandlerTests()
        {
            var chargeCalculator = A.Fake<INotificationChargeCalculator>();
            A.CallTo(() => chargeCalculator.GetValue(A<Guid>.Ignored)).Returns(TotalBillable);
            transactions = new List<NotificationTransaction>();

            var transactionsList = new List<NotificationTransaction>();
            transactionsList.Add(new NotificationTransaction(new NotificationTransactionData
            {
                Date = NewDate,
                NotificationId = NotificationId,
                Credit = 100.00m,
                PaymentMethod = PaymentMethod.BacsChaps
            }));

            var repository = A.Fake<INotificationTransactionRepository>();
            A.CallTo(() => repository.GetTransactions(NotificationId)).Returns(transactions);
            
            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = NotificationId });

            assessment = new NotificationAssessment(NotificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, assessment);
            
            context.NotificationAssessments.Add(assessment);

            context.NotificationApplications.Add(new TestableNotificationApplication { Id = NotificationId });
            
            var assessmentRepository = A.Fake<INotificationAssessmentRepository>();
            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId)).Returns(assessment);

            handler = new AddNotificationTransactionHandler(context, new Transaction(assessmentRepository, repository, new NotificationTransactionCalculator(repository, chargeCalculator)));
        }

        [Fact]
        public async Task PaymentComplete_PaymentReceivedDateNotSet_AddsDateToAssessment()
        {
            var newDate = NewDate;

            var paymentComplete = GetAddNotificationTransaction(newDate, TotalBillable, null);
            await handler.HandleAsync(paymentComplete);

            Assert.Equal(newDate, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task PaymentComplete_PaymentReceivedDateSet_AddsDateToAssessment()
        {
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, existingDate, assessment.Dates);

            var paymentComplete = GetAddNotificationTransaction(NewDate, TotalBillable, null);
            await handler.HandleAsync(paymentComplete);

            Assert.Equal(NewDate, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task PaymentMadeButPaymentIncomplete_PaymentReceivedDateNull()
        {
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, existingDate, assessment.Dates);

            var incompletePayment = GetAddNotificationTransaction(existingDate, 50.00m, null);
            await handler.HandleAsync(incompletePayment);

            Assert.Equal(null, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task RefundMade_PaymentComplete_DoesNotAddDateToAssessment()
        {
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, existingDate, assessment.Dates);

            var paymentComplete = GetAddNotificationTransaction(existingDate, TotalBillable, null);
            await handler.HandleAsync(paymentComplete);

            var refundMade = GetAddNotificationTransaction(existingDate, null, 50.00m);
            await handler.HandleAsync(refundMade);

            Assert.Equal(null, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }
        
        [Fact]
        public async Task PaymentComplete_RefundMade_PaymentCompletedAgain_UpdatesDateToAssessment()
        {
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, existingDate, assessment.Dates);

            var paymentComplete = GetAddNotificationTransaction(existingDate, TotalBillable, null);
            await handler.HandleAsync(paymentComplete);

            var refundMade = GetAddNotificationTransaction(NewDate, null, 50.00m);
            await handler.HandleAsync(refundMade);

            var paymentCompleteAgain = GetAddNotificationTransaction(newerDate, 50.00m, null);
            await handler.HandleAsync(paymentCompleteAgain);

            Assert.Equal(newerDate, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        private AddNotificationTransaction GetAddNotificationTransaction(DateTime date, decimal? credit, decimal? debit)
        {
            var transactionData =
                new NotificationTransactionData
                {
                    Date = date,
                    Credit = credit,
                    Debit = debit,
                    NotificationId = NotificationId,
                    PaymentMethod = PaymentMethod.BacsChaps
                };

            var message = new AddNotificationTransaction(transactionData);

            transactions.Add(new NotificationTransaction(transactionData));

            return message;
        }
    }
}
