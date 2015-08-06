namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Xunit;

    public class FinancialGuaranteeReceivedTests : FinancialGuaranteeTests
    {
        [Fact]
        public void SetReceivedDate_SetsDate()
        {
            FinancialGuarantee.Received(AnyDate);

            Assert.Equal(AnyDate, FinancialGuarantee.ReceivedDate);
        }

        [Fact]
        public void Status_IsPending()
        {
            Assert.Equal(FinancialGuaranteeStatus.AwaitingApplication, FinancialGuarantee.Status);
        }

        [Fact]
        public void SetReceivedDate_ChangesStatus()
        {
            FinancialGuarantee.Received(AnyDate);

            Assert.Equal(FinancialGuaranteeStatus.ApplicationReceived, FinancialGuarantee.Status);
        }

        [Fact]
        public void SetReceivedDateTwice_Throws()
        {
            FinancialGuarantee.Received(AnyDate);

            Assert.Throws<InvalidOperationException>(() => FinancialGuarantee.Received(AnyDate.AddDays(1)));
        }
    }
}
