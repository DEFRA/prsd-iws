namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;

    public class FinancialGuaranteeTests
    {
        private static readonly DateTime AnyDate = new DateTime(2014, 5, 5);
        private readonly FinancialGuarantee financialGuarantee;
        private readonly FinancialGuarantee completedFinancialGuarantee;
        private readonly FinancialGuarantee receivedFinancialGuarantee;

        public FinancialGuaranteeTests()
        {
            financialGuarantee = FinancialGuarantee.Create();

            completedFinancialGuarantee = FinancialGuarantee.Create();
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.ApplicationComplete, completedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, completedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, AnyDate.AddDays(1), completedFinancialGuarantee);

            receivedFinancialGuarantee = FinancialGuarantee.Create();
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.ApplicationReceived, receivedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, receivedFinancialGuarantee);
        }

        [Fact]
        public void SetReceivedDate_SetsDate()
        {
            financialGuarantee.Received(AnyDate);

            Assert.Equal(AnyDate, financialGuarantee.ReceivedDate);
        }

        [Fact]
        public void Status_IsPending()
        {
            Assert.Equal(FinancialGuaranteeStatus.AwaitingApplication, financialGuarantee.Status);
        }

        [Fact]
        public void SetReceivedDate_ChangesStatus()
        {
            financialGuarantee.Received(AnyDate);

            Assert.Equal(FinancialGuaranteeStatus.ApplicationReceived, financialGuarantee.Status);
        }

        [Fact]
        public void SetCompletedDate_NullReceivedDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => financialGuarantee.Completed(AnyDate));
        }

        [Fact]
        public void SetReceivedDateTwice_Throws()
        {
            financialGuarantee.Received(AnyDate);

            Assert.Throws<InvalidOperationException>(() => financialGuarantee.Received(AnyDate.AddDays(1)));
        }

        [Fact]
        public void SetCompletedDate_AfterReceivedDate_SetsDate()
        {
            financialGuarantee.Received(AnyDate);

            financialGuarantee.Completed(AnyDate.AddDays(1));

            Assert.Equal(AnyDate.AddDays(1), financialGuarantee.CompletedDate);
        }

        [Fact]
        public void SetCompletedDate_BeforeReceivedDate_Throws()
        {
            financialGuarantee.Received(AnyDate);

            Assert.Throws<InvalidOperationException>(() => financialGuarantee.Completed(AnyDate.AddDays(-1)));
        }

        [Fact]
        public void Create_GeneratesObjectwithExpectedValues()
        {
            SystemTime.Freeze();

            var fg = FinancialGuarantee.Create();

            Assert.Equal(SystemTime.UtcNow, fg.CreatedDate);
            Assert.Equal(FinancialGuaranteeStatus.AwaitingApplication, fg.Status);

            SystemTime.Unfreeze();
        }

        [Fact]
        public void UpdateReceivedDate_NullReceivedDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => financialGuarantee.UpdateReceivedDate(AnyDate));
        }

        [Fact]
        public void UpdateReceivedDate_GreaterThanCompletedDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => completedFinancialGuarantee.UpdateReceivedDate(AnyDate.AddDays(2)));
        }

        [Fact]
        public void UpdateReceivedDate_SetToSameDate_Passes()
        {
            receivedFinancialGuarantee.UpdateReceivedDate(AnyDate);

            Assert.Equal(AnyDate, receivedFinancialGuarantee.ReceivedDate);
        }

        [Fact]
        public void UpdateReceivedDate_ToEarlierDate_Passes()
        {
            receivedFinancialGuarantee.UpdateReceivedDate(AnyDate.AddDays(-2));

            Assert.Equal(AnyDate.AddDays(-2), receivedFinancialGuarantee.ReceivedDate);
        }

        [Fact]
        public void UpdateReceivedDateForCompleted_ToEarlierDate_Passes()
        {
            completedFinancialGuarantee.UpdateReceivedDate(AnyDate.AddDays(-1));

            Assert.Equal(AnyDate.AddDays(-1), completedFinancialGuarantee.ReceivedDate);
        }

        [Fact]
        public void UpdateCompletedDate_NewRecord_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => financialGuarantee.UpdateCompletedDate(AnyDate));
        }

        [Fact]
        public void UpdateCompletedDate_ReceivedRecord_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => receivedFinancialGuarantee.UpdateCompletedDate(AnyDate));
        }

        [Fact]
        public void UpdateCompletedDate_ToBeforeReceivedDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(
                () => completedFinancialGuarantee.UpdateCompletedDate(AnyDate.AddDays(-1)));
        }

        [Fact]
        public void UpdateCompletedDate_AfterCurrentDate_Updates()
        {
            completedFinancialGuarantee.UpdateCompletedDate(AnyDate.AddDays(3));

            Assert.Equal(AnyDate.AddDays(3), completedFinancialGuarantee.CompletedDate);
        }

        [Fact]
        public void UpdateCompletedDate_ToCurrentDate_Updates()
        {
            completedFinancialGuarantee.UpdateCompletedDate(AnyDate.AddDays(1));

            Assert.Equal(AnyDate.AddDays(1), completedFinancialGuarantee.CompletedDate);
        }
    }
}
