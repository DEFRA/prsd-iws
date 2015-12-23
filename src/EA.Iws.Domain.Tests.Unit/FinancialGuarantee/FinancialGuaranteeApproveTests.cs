namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using Xunit;

    public class FinancialGuaranteeApproveTests : FinancialGuaranteeTests
    {
        private static readonly DateTime TwoDaysAfterCompletionDate = CompletedDate.AddDays(2);
        private static readonly DateTime YearAfterCompletionDate = CompletedDate.AddYears(1);
        private const int AnyInt = 7;
        private const string BlanketBondReference = "ref 23";

        private readonly Action<FinancialGuarantee> setGuaranteeApproved =
            fg =>
                fg.Approve(new ApproveDates(AfterCompletionDate, 
                    TwoDaysAfterCompletionDate, 
                    YearAfterCompletionDate,
                    BlanketBondReference, 
                    AnyInt,
                    AnyInt));

        [Fact]
        public void StatusNotCompletedThrows()
        {
            Assert.Throws<InvalidOperationException>(() => setGuaranteeApproved(ReceivedFinancialGuarantee));
        }

        [Fact]
        public void DecisionDateBeforeCompletedDateThrows()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                    CompletedFinancialGuarantee.Approve(new ApproveDates(BeforeCompletionDate, AfterCompletionDate,
                        TwoDaysAfterCompletionDate, BlanketBondReference, AnyInt, AnyInt)));
        }

        [Fact]
        public void ValidFromBeforeValidToThrows()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                    CompletedFinancialGuarantee.Approve(new ApproveDates(AfterCompletionDate, YearAfterCompletionDate,
                        TwoDaysAfterCompletionDate, BlanketBondReference, AnyInt, AnyInt)));
        }

        [Fact]
        public void SetsDecisionDate()
        {
            setGuaranteeApproved(CompletedFinancialGuarantee);

            Assert.Equal(CompletedDate.AddDays(1), CompletedFinancialGuarantee.DecisionDate);
        }

        [Fact]
        public void ChangesStatus()
        {
            setGuaranteeApproved(CompletedFinancialGuarantee);

            Assert.Equal(FinancialGuaranteeStatus.Approved, CompletedFinancialGuarantee.Status);
        }

        [Fact]
        public void SetsApprovedFromAndApprovedTo()
        {
            setGuaranteeApproved(CompletedFinancialGuarantee);

            Assert.Equal(TwoDaysAfterCompletionDate, CompletedFinancialGuarantee.ApprovedFrom);
            Assert.Equal(YearAfterCompletionDate, CompletedFinancialGuarantee.ApprovedTo);
        }

        [Fact]
        public void ValidFromBeforeValidTo_DoesNotSetStatusAndDoesNotRaiseEvent()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                    CompletedFinancialGuarantee.Approve(new ApproveDates(AfterCompletionDate, YearAfterCompletionDate, TwoDaysAfterCompletionDate, BlanketBondReference, AnyInt, AnyInt)));

            Assert.Equal(FinancialGuaranteeStatus.ApplicationComplete, CompletedFinancialGuarantee.Status);
            A.CallTo(() => Dispatcher.Dispatch(A<FinancialGuaranteeStatusChangeEvent>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public void SetsActiveLoadsPermitted()
        {
            setGuaranteeApproved(CompletedFinancialGuarantee);

            Assert.Equal(AnyInt, CompletedFinancialGuarantee.ActiveLoadsPermitted);
        }
    }
}
