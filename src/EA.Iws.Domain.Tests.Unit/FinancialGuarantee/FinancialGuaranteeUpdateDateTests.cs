namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Xunit;

    public class FinancialGuaranteeUpdateDateTests : FinancialGuaranteeTests
    {
        [Fact]
        public void UpdateReceivedDate_GreaterThanCompletedDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => CompletedFinancialGuarantee.UpdateReceivedDate(AnyDate.AddDays(2)));
        }

        [Fact]
        public void UpdateReceivedDate_SetToSameDate_Passes()
        {
            ReceivedFinancialGuarantee.UpdateReceivedDate(AnyDate);

            Assert.Equal(AnyDate, ReceivedFinancialGuarantee.ReceivedDate);
        }

        [Fact]
        public void UpdateReceivedDate_ToEarlierDate_Passes()
        {
            ReceivedFinancialGuarantee.UpdateReceivedDate(AnyDate.AddDays(-2));

            Assert.Equal(AnyDate.AddDays(-2), ReceivedFinancialGuarantee.ReceivedDate);
        }

        [Fact]
        public void UpdateReceivedDateForCompleted_ToEarlierDate_Passes()
        {
            CompletedFinancialGuarantee.UpdateReceivedDate(AnyDate.AddDays(-1));

            Assert.Equal(AnyDate.AddDays(-1), CompletedFinancialGuarantee.ReceivedDate);
        }

        [Fact]
        public void UpdateCompletedDate_NewRecord_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => FinancialGuarantee.UpdateCompletedDate(AnyDate));
        }

        [Fact]
        public void UpdateCompletedDate_ReceivedRecord_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => ReceivedFinancialGuarantee.UpdateCompletedDate(AnyDate));
        }

        [Fact]
        public void UpdateCompletedDate_ToBeforeReceivedDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(
                () => CompletedFinancialGuarantee.UpdateCompletedDate(AnyDate.AddDays(-1)));
        }

        [Fact]
        public void UpdateCompletedDate_AfterCurrentDate_Updates()
        {
            CompletedFinancialGuarantee.UpdateCompletedDate(AnyDate.AddDays(3));

            Assert.Equal(AnyDate.AddDays(3), CompletedFinancialGuarantee.CompletedDate);
        }

        [Fact]
        public void UpdateCompletedDate_ToCurrentDate_Updates()
        {
            CompletedFinancialGuarantee.UpdateCompletedDate(AnyDate.AddDays(1));

            Assert.Equal(AnyDate.AddDays(1), CompletedFinancialGuarantee.CompletedDate);
        }
    }
}
