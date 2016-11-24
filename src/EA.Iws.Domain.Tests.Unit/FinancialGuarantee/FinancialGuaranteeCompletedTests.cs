namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Prsd.Core;
    using Xunit;

    public class FinancialGuaranteeCompletedTests : FinancialGuaranteeTests
    {
        [Fact]
        public void SetCompletedDate_NullReceivedDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => FinancialGuarantee.Completed(AnyDate));
        }

        [Fact]
        public void SetCompletedDate_AfterReceivedDate_SetsDate()
        {
            FinancialGuarantee.Received(AnyDate);

            FinancialGuarantee.Completed(AnyDate.AddDays(1));

            Assert.Equal(AnyDate.AddDays(1), FinancialGuarantee.CompletedDate);
        }

        [Fact]
        public void SetCompletedDate_BeforeReceivedDate_Throws()
        {
            FinancialGuarantee.Received(AnyDate);

            Assert.Throws<InvalidOperationException>(() => FinancialGuarantee.Completed(AnyDate.AddDays(-1)));
        }

        [Fact]
        public void Create_GeneratesObjectwithExpectedValues()
        {
            SystemTime.Freeze();

            var fg = FinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);

            Assert.Equal(SystemTime.UtcNow, fg.CreatedDate);
            Assert.Equal(FinancialGuaranteeStatus.AwaitingApplication, fg.Status);

            SystemTime.Unfreeze();
        }
    }
}
