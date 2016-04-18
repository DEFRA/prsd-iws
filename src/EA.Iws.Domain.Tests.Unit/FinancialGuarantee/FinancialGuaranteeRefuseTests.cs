namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using Domain.FinancialGuarantee;
    using System;
    using Core.FinancialGuarantee;
    using Xunit;

    public class FinancialGuaranteeRefuseTests : FinancialGuaranteeTests
    {
        private readonly Action<FinancialGuarantee, DateTime> refuseGuarantee =
            (guarantee, date) => guarantee.Refuse(date, AnyString);
        
        [Fact]
        public void FinancialGuaranteeNotCompletedThrows()
        {
            Assert.Throws<InvalidOperationException>(() => refuseGuarantee(ReceivedFinancialGuarantee, AnyDate));
        }

        [Fact]
        public void DecisionDateBeforeCompletedDateThrows()
        {
            Assert.Throws<InvalidOperationException>(() => refuseGuarantee(CompletedFinancialGuarantee, BeforeCompletionDate));
        }

        [Fact]
        public void DecisionDateIsSet()
        {
            refuseGuarantee(CompletedFinancialGuarantee, AfterCompletionDate);

            Assert.Equal(AfterCompletionDate, CompletedFinancialGuarantee.DecisionDate);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void RefusalReasonNullOrEmptyThrows(string refusalReason)
        {
            Assert.Throws<ArgumentException>(() => CompletedFinancialGuarantee.Refuse(AfterCompletionDate, refusalReason));
        }

        [Fact]
        public void RefusalReasonNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => CompletedFinancialGuarantee.Refuse(AfterCompletionDate, null));
        }

        [Fact]
        public void DecisionDateBeforeCompletionDate_DoesNotSetDecisionDateAndStatus()
        {
            Assert.Throws<InvalidOperationException>(() => refuseGuarantee(CompletedFinancialGuarantee, BeforeCompletionDate));

            Assert.False(CompletedFinancialGuarantee.DecisionDate.HasValue);
            Assert.Equal(FinancialGuaranteeStatus.ApplicationComplete, CompletedFinancialGuarantee.Status);
        }

        [Fact]
        public void StatusIsSet()
        {
            refuseGuarantee(CompletedFinancialGuarantee, AfterCompletionDate);

            Assert.Equal(FinancialGuaranteeStatus.Refused, CompletedFinancialGuarantee.Status);
        }

        [Fact]
        public void RefusalReasonIsSet()
        {
            CompletedFinancialGuarantee.Refuse(AfterCompletionDate, AnyString);

            Assert.Equal(AnyString, CompletedFinancialGuarantee.RefusalReason);
        }
    }
}
