namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using Core.FinancialGuarantee;
    using Prsd.Core;
    using Xunit;

    public class FinancialGuaranteeReceivedTests : FinancialGuaranteeTests
    {
        [Fact]
        public void Create_GeneratesObjectwithExpectedValues()
        {
            SystemTime.Freeze();

            var fg = FinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);

            Assert.Equal(SystemTime.UtcNow, fg.CreatedDate);
            Assert.Equal(FinancialGuaranteeStatus.ApplicationReceived, fg.Status);

            SystemTime.Unfreeze();
        }

        [Fact]
        public void ReceivedDate_IsSet()
        {
            Assert.Equal(AnyDate, FinancialGuarantee.ReceivedDate);
        }

        [Fact]
        public void Status_IsApplicationReceived()
        {
            Assert.Equal(FinancialGuaranteeStatus.ApplicationReceived, FinancialGuarantee.Status);
        }

        [Fact]
        public void Decision_IsNone()
        {
            Assert.Equal(FinancialGuaranteeDecision.None, FinancialGuarantee.Decision);
        }
    }
}
