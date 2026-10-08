namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Transactions;
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

            A.CallTo(() => importNotificationTransactionCalculator.TotalBillable(notificationId))
                .Returns(1000m);

            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(new List<ImportNotificationTransaction>());

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
            var transactionDate = new DateTime(2018, 1, 1);

            A.CallTo(() => importNotificationTransactionCalculator.TotalBillable(notificationId))
                .Returns(1000m);
            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(new List<ImportNotificationTransaction>());

            await transaction.Save(notificationId, transactionDate, 1000, PaymentMethod.Card, null, null);

            Assert.Equal(transactionDate, assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Save_PaymentNotFullyReceived_ReceivedDateNull()
        {
            A.CallTo(() => importNotificationTransactionCalculator.TotalBillable(notificationId))
                .Returns(1000m);

            await transaction.Save(notificationId, new DateTime(2018, 1, 1), 999, PaymentMethod.Card, null, null);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Save_PartialPaymentsThenFullPayment_SetsCorrectReceivedDate()
        {
            var firstPaymentDate = new DateTime(2018, 1, 1);
            var secondPaymentDate = new DateTime(2018, 2, 1);

            A.CallTo(() => importNotificationTransactionCalculator.TotalBillable(notificationId))
                .Returns(1000m);
            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(new List<ImportNotificationTransaction>
                {
                    CreateNotificationTransaction(600, firstPaymentDate)
                });

            await transaction.Save(notificationId, secondPaymentDate, 400, PaymentMethod.Card, null, null);

            Assert.Equal(secondPaymentDate, assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Save_NonAwaitingPaymentStatus_SetsReceivedDateDirectly()
        {
            var transactionDate = new DateTime(2025, 7, 14);

            A.CallTo(() => importNotificationTransactionCalculator.TotalBillable(notificationId))
                .Returns(6700m);
            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(new List<ImportNotificationTransaction>());

            await transaction.Save(notificationId, transactionDate, 6700, PaymentMethod.Card, null, null);

            Assert.Equal(transactionDate, assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentNotFullyReceived_ReceivedDateNull()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var notificationTransaction = CreateNotificationTransaction(600, new DateTime(2018, 1, 1));

            A.CallTo(() => importNotificationTransactionRepository.GetById(transactionId))
                .Returns(notificationTransaction);

            A.CallTo(() => importNotificationTransactionCalculator.TotalBillable(notificationId))
                .Returns(1000m);
            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(new List<ImportNotificationTransaction> { notificationTransaction });

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            await transaction.Delete(notificationId, transactionId);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentStillFullyReceived_ReceivedDateUpdated()
        {
            var expectedPaymentDate = new DateTime(2018, 2, 2);
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var transactionToDelete = CreateNotificationTransaction(100, new DateTime(2018, 4, 4));
            var transactionRemaining = CreateNotificationTransaction(1000, expectedPaymentDate);

            A.CallTo(() => importNotificationTransactionRepository.GetById(transactionId))
                .Returns(transactionToDelete);

            A.CallTo(() => importNotificationTransactionCalculator.TotalBillable(notificationId))
                .Returns(1000m);
            A.CallTo(() => importNotificationTransactionRepository.GetTransactions(notificationId))
                .Returns(new List<ImportNotificationTransaction> { transactionToDelete, transactionRemaining });

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            await transaction.Delete(notificationId, transactionId);

            Assert.Equal(expectedPaymentDate, assessment.Dates.PaymentReceivedDate);
        }
    }
}