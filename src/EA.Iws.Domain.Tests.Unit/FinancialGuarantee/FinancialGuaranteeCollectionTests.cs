namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Domain.FinancialGuarantee;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;

    public class FinancialGuaranteeCollectionTests : IDisposable
    {
        private readonly Guid approvedFinancialGuaranteeId = new Guid("C895C5FE-2C82-4AF9-B624-C48499A342B3");
        private readonly FinancialGuaranteeCollection financialGuaranteeCollection;
        private readonly Guid notificationId = new Guid("EC55CFDC-76E8-44A2-B7B1-322619F7F289");
        private readonly DateTime today = new DateTime(2016, 12, 1);

        public FinancialGuaranteeCollectionTests()
        {
            SystemTime.Freeze(today);

            financialGuaranteeCollection = new FinancialGuaranteeCollection(notificationId);

            var approvedFinancialGuarantee = financialGuaranteeCollection.AddFinancialGuarantee(today.AddDays(-10));
            EntityHelper.SetEntityId(approvedFinancialGuarantee, approvedFinancialGuaranteeId);
            approvedFinancialGuarantee.Complete(today.AddDays(-9));
            approvedFinancialGuarantee.Approve(new ApprovalData(today.AddDays(-8), "123", 10, false));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void GetCurrentApprovedFinancialGuarantee_DoesntReturnReleased()
        {
            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(approvedFinancialGuaranteeId);
            financialGuarantee.Release(today);

            Assert.Null(financialGuaranteeCollection.GetCurrentApprovedFinancialGuarantee());
        }

        [Fact]
        public void GetCurrentApprovedFinancialGuarantee_ReturnsExpected()
        {
            Assert.Equal(approvedFinancialGuaranteeId, financialGuaranteeCollection.GetCurrentApprovedFinancialGuarantee().Id);
        }

        [Fact]
        public void GetCurrentApprovedFinancialGuarantee_NewGuaranteeCreated_ReturnsExpected()
        {
            var newFinancialGuarantee = financialGuaranteeCollection.AddFinancialGuarantee(today);
            newFinancialGuarantee.Complete(today);

            Assert.Equal(approvedFinancialGuaranteeId, financialGuaranteeCollection.GetCurrentApprovedFinancialGuarantee().Id);
        }
    }
}