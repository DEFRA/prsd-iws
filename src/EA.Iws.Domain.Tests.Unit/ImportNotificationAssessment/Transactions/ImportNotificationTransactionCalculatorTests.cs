namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Transactions;
    using FakeItEasy;
    using Xunit;

    public class ImportNotificationTransactionCalculatorTests
    {
        private const int TotalDebits = 21;
        private const int TotalCredits = 52;

        private readonly IImportNotificationChargeCalculator chargeCalculator;
        private readonly IImportNotificationTransactionRepository repository;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IEnumerable<ImportNotificationTransaction> transactions = new[]
        {
            ImportNotificationTransaction.PaymentRecord(Guid.Empty, DateTime.MaxValue, 15,
                PaymentMethod.BacsChaps, null, null),
            ImportNotificationTransaction.PaymentRecord(Guid.Empty, DateTime.MaxValue, 37,
                PaymentMethod.BacsChaps, null, null),
            ImportNotificationTransaction.RefundRecord(Guid.Empty, DateTime.MaxValue, TotalDebits, null)
        };

        public ImportNotificationTransactionCalculatorTests()
        {
            chargeCalculator = A.Fake<IImportNotificationChargeCalculator>();
            repository = A.Fake<IImportNotificationTransactionRepository>();
            transactionCalculator = new ImportNotificationTransactionCalculator(repository, chargeCalculator);
            A.CallTo(() => repository.GetTransactions(A<Guid>.Ignored)).Returns(transactions);
            A.CallTo(() => chargeCalculator.GetValue(A<Guid>.Ignored)).Returns(TotalCredits);
        }

        [Fact]
        public void TotalCredits_ReturnsCorrectValue()
        {
            Assert.Equal(TotalCredits, transactionCalculator.TotalCredits(transactions));
        }

        [Fact]
        public void TotalCredits_NullEnumerable_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => transactionCalculator.TotalCredits(null));
        }

        [Fact]
        public void TotalDebits_ReturnsCorrectValue()
        {
            Assert.Equal(TotalDebits, transactionCalculator.TotalDebits(transactions));
        }

        [Fact]
        public void TotalDebits_NullEnumerable_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => transactionCalculator.TotalDebits(null));
        }

        [Fact]
        public async Task Balance_ReturnsCorrectValue()
        {
            Assert.Equal(TotalCredits - TotalDebits, await transactionCalculator.Balance(Guid.Empty));
        }

        [Fact]
        public async Task PaymentIsNowFullyReceived_PaymentReceived_ReturnsTrue()
        {
            var isFullyReceived = await transactionCalculator.PaymentIsNowFullyReceived(Guid.Empty, TotalDebits);

            Assert.True(isFullyReceived);
        }

        [Fact]
        public async Task PaymentIsNowFullyReceived_PaymentNotReceived_ReturnsFalse()
        {
            var isFullyReceived = await transactionCalculator.PaymentIsNowFullyReceived(Guid.Empty, 0);

            Assert.False(isFullyReceived);
        }
    }
}
