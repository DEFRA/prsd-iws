﻿namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class TransactionTests
    {
        private readonly Transaction transaction;
        private readonly TestableNotificationAssessment assessment;
        private readonly Guid notificationId = new Guid("34C335B5-1329-47E1-8605-DE5532ADF9D7");

        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationTransactionRepository notificationTransactionRepository;
        private readonly INotificationTransactionCalculator notificationTransactionCalculator;

        public TransactionTests()
        {
            notificationAssessmentRepository = A.Fake<INotificationAssessmentRepository>();
            notificationTransactionRepository = A.Fake<INotificationTransactionRepository>();
            notificationTransactionCalculator = A.Fake<INotificationTransactionCalculator>();

            assessment = new TestableNotificationAssessment();
            assessment.NotificationApplicationId = notificationId;
            assessment.Dates = new NotificationDates();

            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(notificationId))
                .Returns(assessment);

            A.CallTo(() => notificationTransactionCalculator.Balance(notificationId))
                .Returns(1000);

            transaction = new Transaction(
                notificationAssessmentRepository,
                notificationTransactionRepository,
                notificationTransactionCalculator);
        }

        private NotificationTransaction CreateNotificationTransaction(decimal credit, DateTime? date = null)
        {
            return new NotificationTransaction(
                new NotificationTransactionData
                {
                    Credit = credit,
                    Date = date == null ? new DateTime(2017, 1, 1) : date.GetValueOrDefault(),
                    NotificationId = notificationId,
                    PaymentMethod = PaymentMethod.Card
                });
        }

        [Fact]
        public async Task Save_AddsNotificationTransaction()
        {
            var notificationTransaction = CreateNotificationTransaction(100);

            await transaction.Save(notificationTransaction);

            A.CallTo(() => notificationTransactionRepository.Add(notificationTransaction))
                .MustHaveHappened();
        }

        [Fact]
        public async Task Save_PaymentFullyReceived_SetsReceivedDate()
        {
            var notificationTransaction = CreateNotificationTransaction(1000, new DateTime(2017, 1, 1));

            // Set payment to be the same amount of the transaction
            A.CallTo(() => notificationTransactionCalculator.Balance(notificationId))
                .Returns(1000);
            A.CallTo(() => notificationTransactionRepository.GetTransactions(notificationId))
                .Returns(new List<NotificationTransaction>() { notificationTransaction });

            await transaction.Save(notificationTransaction);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2017, 1, 1));
        }

        [Fact]
        public async Task Save_PaymentNotFullyReceived_ReceivedDateNull()
        {
            var notificationTransaction = CreateNotificationTransaction(999);

            await transaction.Save(notificationTransaction);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentNotFullyReceived_ReceivedDateNull()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var notificationTransaction = CreateNotificationTransaction(600);

            A.CallTo(() => notificationTransactionRepository.GetById(transactionId))
                .Returns(notificationTransaction);

            // Set payment to fully received
            A.CallTo(() => notificationTransactionCalculator.Balance(notificationId))
                .Returns(0);

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            // Delete payment, balance now £600
            await transaction.Delete(notificationId, transactionId);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentStillFullyReceived_ReceivedDateUpdated()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var firstTransaction = CreateNotificationTransaction(100);
            var secondTransaction = CreateNotificationTransaction(200);

            A.CallTo(() => notificationTransactionRepository.GetById(transactionId))
                .Returns(firstTransaction);

            // Set payment to overpaid
            A.CallTo(() => notificationTransactionCalculator.Balance(notificationId))
                .Returns(-200);
            A.CallTo(() => notificationTransactionRepository.GetTransactions(notificationId))
                .Returns(new List<NotificationTransaction>() { secondTransaction });

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            // Delete payment, balance now -£100
            await transaction.Delete(notificationId, transactionId);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2017, 1, 1));
        }
    }
}
