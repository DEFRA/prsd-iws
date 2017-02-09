namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment.Transactions
{
    using System;
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

            transaction = new ImportPaymentTransaction(
                importNotificationTransactionRepository,
                importNotificationTransactionCalculator,
                importNotificationAssessmentRepository);
        }

        private ImportNotificationTransaction CreateNotificationTransaction(int credit)
        {
            return ImportNotificationTransaction.PaymentRecord(notificationId, new DateTime(2017, 1, 1), credit,
                PaymentMethod.Card, null, null);
        }

        [Fact]
        public async Task Delete_PaymentNotFullyReceived_ReceivedDateNull()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var notificationTransaction = CreateNotificationTransaction(600);

            A.CallTo(() => importNotificationTransactionRepository.GetById(transactionId))
                .Returns(notificationTransaction);

            // Set payment to fully received
            A.CallTo(() => importNotificationTransactionCalculator.Balance(notificationId))
                .Returns(0);

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            // Delete payment, balance now £600
            await transaction.Delete(notificationId, transactionId);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentStillFullyReceived_ReceivedDateRemains()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var notificationTransaction = CreateNotificationTransaction(100);

            A.CallTo(() => importNotificationTransactionRepository.GetById(transactionId))
                .Returns(notificationTransaction);

            // Set payment to overpaid
            A.CallTo(() => importNotificationTransactionCalculator.Balance(notificationId))
                .Returns(-100);

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            // Delete payment, balance now £0
            await transaction.Delete(notificationId, transactionId);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2017, 2, 2));
        }
    }
}