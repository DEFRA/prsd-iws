namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Xunit;

    public class FinancialGuaranteeReleaseTests : FinancialGuaranteeTests
    {
        private readonly Action<FinancialGuarantee> releaseGuarantee =
            guarantee => guarantee.Release(AfterCompletionDate);
        
        [Fact]
        public void StateNotCompletedThrows()
        {
            Assert.Throws<InvalidOperationException>(() => releaseGuarantee(ReceivedFinancialGuarantee));
        }

        [Fact]
        public void DecisionDateBeforeCompletionDateThrows()
        {
            Assert.Throws<InvalidOperationException>(() => ApprovedFinancialGuarantee.Release(BeforeCompletionDate));
        }

        [Fact]
        public void SetsReleasedDate()
        {
            ApprovedFinancialGuarantee.Release(AfterCompletionDate);

            Assert.Equal(AfterCompletionDate, ApprovedFinancialGuarantee.ReleasedDate);
        }

        [Fact]
        public void CannotReleaseCompletedGuarantee()
        {
            Assert.Throws<InvalidOperationException>(() => CompletedFinancialGuarantee.Release(AfterCompletionDate));
        }

        [Fact]
        public void DoesNotOverrideDecisionDate()
        {
            var previousDecisionDate = ApprovedFinancialGuarantee.DecisionDate.Value;
            var afterDecisionDate = previousDecisionDate.AddDays(1);

            ApprovedFinancialGuarantee.Release(afterDecisionDate);

            Assert.Equal(previousDecisionDate, ApprovedFinancialGuarantee.DecisionDate);
        }

        [Fact]
        public void SetsStatus()
        {
            releaseGuarantee(ApprovedFinancialGuarantee);

            Assert.Equal(FinancialGuaranteeStatus.Released, ApprovedFinancialGuarantee.Status);
        }

        [Fact]
        public void CanReleaseAnApprovedGuarantee()
        {
            ApprovedFinancialGuarantee.Release(AfterCompletionDate);

            Assert.Equal(FinancialGuaranteeStatus.Released, ApprovedFinancialGuarantee.Status);
        }

        [Fact]
        public void CanReleaseARefusedGuarantee()
        {
            RefusedFinancialGuarantee.Release(AfterCompletionDate);

            Assert.Equal(FinancialGuaranteeStatus.Released, RefusedFinancialGuarantee.Status);
        }

        [Fact]
        public void ReleaseApprovedRetainsDecision()
        {
            ApprovedFinancialGuarantee.Release(AfterCompletionDate);

            Assert.Equal(FinancialGuaranteeDecision.Approved, ApprovedFinancialGuarantee.Decision);
        }
    }
}
