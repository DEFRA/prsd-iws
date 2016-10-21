namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotificationAssessment.Transactions;
    using FakeItEasy;
    using Prsd.Core;
    using Xunit;

    public class ImportRefundTransactionTests : IDisposable
    {
        private readonly ImportRefundTransaction refundTransaction;
        private readonly IImportNotificationTransactionRepository transactionRepository;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly Guid notificationId;

        public ImportRefundTransactionTests()
        {
            transactionRepository = A.Fake<IImportNotificationTransactionRepository>();
            transactionCalculator = A.Fake<IImportNotificationTransactionCalculator>();
            refundTransaction = new ImportRefundTransaction(transactionRepository, transactionCalculator);
            notificationId = new Guid("DB476D01-2870-4322-8284-520B34D9667B");

            A.CallTo(() => transactionCalculator.TotalPaid(notificationId)).Returns(100);
            A.CallTo(() => transactionRepository.GetTransactions(notificationId))
                .Returns(new[]
                {
                    ImportNotificationTransaction.PaymentRecord(notificationId, new DateTime(2015, 12, 1), 100,
                        PaymentMethod.Cheque, "12345", "comments"),
                });

            SystemTime.Freeze(new DateTime(2016, 1, 1));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task RefundAmountCannotExceedAmountPaid()
        {
            Func<Task> testCode = () => refundTransaction.Save(notificationId, new DateTime(2016, 1, 1), 101, "comment");

            await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        }

        [Fact]
        public async Task RefundAmountCanEqualAmountPaid()
        {
            await refundTransaction.Save(notificationId, new DateTime(2016, 1, 1), 100, "comment");

            A.CallTo(() => transactionRepository.Add(A<ImportNotificationTransaction>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task RefundAmountCanBeLessThanAmountPaid()
        {
            await refundTransaction.Save(notificationId, new DateTime(2016, 1, 1), 99, "comment");

            A.CallTo(() => transactionRepository.Add(A<ImportNotificationTransaction>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task RefundDateCanBeToday()
        {
            await refundTransaction.Save(notificationId, new DateTime(2016, 1, 1), 99, "comment");

            A.CallTo(() => transactionRepository.Add(A<ImportNotificationTransaction>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task RefundDateCanBeInThePast()
        {
            await refundTransaction.Save(notificationId, new DateTime(2015, 12, 31), 99, "comment");

            A.CallTo(() => transactionRepository.Add(A<ImportNotificationTransaction>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task RefundDateCannotBeInTheFuture()
        {
            Func<Task> testCode = () => refundTransaction.Save(notificationId, new DateTime(2016, 1, 2), 99, "comment");

            await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        }

        [Fact]
        public async Task RefundDateCannotBeBeforeFirstPaymentDate()
        {
            Func<Task> testCode = () => refundTransaction.Save(notificationId, new DateTime(2015, 11, 30), 99, "comment");

            await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        }

        [Fact]
        public async Task CommentsCanBeNull()
        {
            await refundTransaction.Save(notificationId, new DateTime(2015, 12, 31), 99, null);

            A.CallTo(() => transactionRepository.Add(A<ImportNotificationTransaction>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task CantRefundWhenNoPaymentsMade()
        {
            A.CallTo(() => transactionRepository.GetTransactions(notificationId))
                .Returns(new ImportNotificationTransaction[] { });

            Func<Task> testCode = () => refundTransaction.Save(notificationId, new DateTime(2015, 12, 2), 99, "comment");

            await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        }
    }
}