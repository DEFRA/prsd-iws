namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Transactions;
    using FakeItEasy;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;

    public class ImportRefundTransactionTests : IDisposable
    {
        private readonly ImportRefundTransaction refundTransaction;
        private readonly IImportNotificationTransactionRepository transactionRepository;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly Guid notificationId;

        public ImportRefundTransactionTests()
        {
            transactionRepository = A.Fake<IImportNotificationTransactionRepository>();
            transactionCalculator = A.Fake<IImportNotificationTransactionCalculator>();
            assessmentRepository = A.Fake<IImportNotificationAssessmentRepository>();
            refundTransaction = new ImportRefundTransaction(transactionRepository, transactionCalculator, assessmentRepository);
            notificationId = new Guid("DB476D01-2870-4322-8284-520B34D9667B");

            var dates = new ImportNotificationDates();
            dates.PaymentReceivedDate = new DateTime(2015, 12, 1);

            var assessment = new ImportNotificationAssessment(notificationId);
            ObjectInstantiator<ImportNotificationAssessment>.SetProperty(x => x.Dates, dates, assessment);

            A.CallTo(() => assessmentRepository.GetByNotification(notificationId)).Returns(assessment);
            A.CallTo(() => transactionCalculator.TotalPaid(notificationId)).Returns(100);

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
        public async Task RefundDateCannotBeBeforePaymentReceivedDate()
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
        public async Task CanRefundWithoutAssessmentDates()
        {
            var assessment = new ImportNotificationAssessment(notificationId);
            A.CallTo(() => assessmentRepository.GetByNotification(notificationId)).Returns(assessment);

            await refundTransaction.Save(notificationId, new DateTime(2016, 1, 1), 100, "comment");

            A.CallTo(() => transactionRepository.Add(A<ImportNotificationTransaction>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}