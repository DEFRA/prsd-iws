namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Xunit;

    public class FinancialGuaranteeCompletedTests : FinancialGuaranteeTests
    {
        [Fact]
        public void SetCompletedDate_BeforeReceivedDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => FinancialGuarantee.Complete(AnyDate.AddDays(-1)));
        }

        [Fact]
        public void SetCompletedDate_AfterReceivedDate_SetsDate()
        {
            FinancialGuarantee.Complete(AnyDate.AddDays(1));

            Assert.Equal(AnyDate.AddDays(1), FinancialGuarantee.CompletedDate);
        }
    }
}
