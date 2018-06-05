namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Transactions;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class ImportPaymentTransactionTests
    {
        private readonly ImportNotificationAssessment assessment;
        private readonly IImportNotificationAssessmentRepository importNotificationAssessmentRepository;
        private readonly IImportNotificationTransactionCalculator importNotificationTransactionCalculator;
        private readonly IImportNotificationTransactionRepository importNotificationTransactionRepository;
        private readonly Guid notificationId = new Guid("6EEC4CDF-3CB0-4145-A8F3-5103263031AA");
        private readonly ImportPaymentTransaction transaction;

        public ImportPaymentTransactionTests()
        {
            importNotificationTransactionRepository = A.Fake<IImportNotificationTransactionRepository>();
            importNotificationTransactionCalculator = A.Fake<IImportNotificationTransactionCalculator>();
            importNotificationAssessmentRepository = A.Fake<IImportNotificationAssessmentRepository>();

            assessment = new ImportNotificationAssessment(notificationId);

            A.CallTo(() => importNotificationAssessmentRepository.GetByNotification(notificationId))
                .Returns(assessment);

            A.CallTo(() => importNotificationTransactionCalculator.Balance(notificationId))
                .Returns(1000);

            transaction = new ImportPaymentTransaction(
                importNotificationTransactionRepository,
                importNotificationTransactionCalculator,
                importNotificationAssessmentRepository);
        }

        private ImportNotificationTransaction CreateNotificationTransaction(int credit, DateTime date)
        {
            return ImportNotificationTransaction.PaymentRecord(notificationId, date, credit,
                PaymentMethod.Card, null, null);
        }

        [Fact]
        public async Task Save_PaymentFullyReceived_SetsReceivedDate()
        {
            var transactionsList = new List<ImportNotificationTransaction>
            {
                CreateNotificationTransaction(1000, new DateTime(2018, 1, 1))
            };

            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            await transaction.Save(notificationId, new DateTime(2018, 1, 1), 1000, PaymentMethod.Card, null, null);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2018, 1, 1));
        }

        [Fact]
        public async Task Save_PaymentFullyReceived_CorrectPaymentReceivedDate()
        {
            var transactionsList = new List<ImportNotificationTransaction>
            {
                CreateNotificationTransaction(300, new DateTime(2018, 1, 1)),
                CreateNotificationTransaction(300, new DateTime(2018, 2, 2)),
                // This will be the payment that reaches 1000 when ordered by date.
                CreateNotificationTransaction(400, new DateTime(2018, 3, 3)),
                CreateNotificationTransaction(300, new DateTime(2018, 4, 4))
            };

            // Set payment to fully received
            A.CallTo(() => importNotificationTransactionCalculator.Balance(notificationId))
                .Returns(0);

            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            await transaction.Save(notificationId, new DateTime(2018, 4, 4), 300, PaymentMethod.Card, null, null);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2018, 3, 3));
        }

        [Fact]
        public async Task Save_PaymentNotFullyReceived_ReceivedDateNull()
        {
            var transactionsList = new List<ImportNotificationTransaction>
            {
                CreateNotificationTransaction(999, new DateTime(2018, 1, 1))
            };

            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            await transaction.Save(notificationId, new DateTime(2018, 1, 1), 999, PaymentMethod.Card, null, null);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentNotFullyReceived_ReceivedDateNull()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var notificationTransaction = CreateNotificationTransaction(600, new DateTime(2018, 1, 1));
            var transactionsList = new List<ImportNotificationTransaction> { notificationTransaction };

            A.CallTo(() => importNotificationTransactionRepository.GetById(transactionId))
                .Returns(notificationTransaction);

            // Set payment to fully received
            A.CallTo(() => importNotificationTransactionCalculator.Balance(notificationId))
                .Returns(0);

            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            // Delete payment, balance now £600
            await transaction.Delete(notificationId, transactionId);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentStillFullyReceived_ReceivedDateUpdated()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var notificationTransaction = CreateNotificationTransaction(100, new DateTime(2018, 4, 4));
            var transactionsList = new List<ImportNotificationTransaction>
            {
                CreateNotificationTransaction(300, new DateTime(2018, 1, 1)),
                // This will be the payment that reaches 1000 when ordered by date.
                CreateNotificationTransaction(700, new DateTime(2018, 2, 2)),
                CreateNotificationTransaction(100, new DateTime(2018, 3, 3))
            };

            A.CallTo(() => importNotificationTransactionRepository.GetById(transactionId))
                .Returns(notificationTransaction);

            // Set payment to overpaid
            A.CallTo(() => importNotificationTransactionCalculator.Balance(notificationId))
                .Returns(-200);

            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            // Delete payment, balance now -£100
            await transaction.Delete(notificationId, transactionId);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2018, 2, 2));
        }
    }
}