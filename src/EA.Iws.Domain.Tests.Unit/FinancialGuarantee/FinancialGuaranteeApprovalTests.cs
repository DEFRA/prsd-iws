namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;

    public class FinancialGuaranteeApprovalTests : IDisposable
    {
        private readonly IFinancialGuaranteeRepository repository;
        private readonly FinancialGuaranteeApproval approval;

        private readonly Guid notificationId = new Guid("8616088F-6AE9-4F44-9DBB-B15287995600");
        private readonly Guid approvedFinancialGuaranteeId = new Guid("075A8FBB-63CE-4E6D-8E02-31F4F1C15F94");
        private readonly Guid financialGuaranteeId = new Guid("969FB203-99E0-4A97-9DC8-F608CC383390");
        private readonly DateTime today = new DateTime(2016, 11, 1);
        private readonly FinancialGuaranteeCollection financialGuaranteeCollection;

        public FinancialGuaranteeApprovalTests()
        {
            SystemTime.Freeze(today);

            repository = A.Fake<IFinancialGuaranteeRepository>();

            financialGuaranteeCollection = new FinancialGuaranteeCollection(notificationId);

            var approvedFinancialGuarantee = financialGuaranteeCollection.AddFinancialGuarantee(today.AddDays(-10));
            EntityHelper.SetEntityId(approvedFinancialGuarantee, approvedFinancialGuaranteeId);
            approvedFinancialGuarantee.Complete(today.AddDays(-9));
            approvedFinancialGuarantee.Approve(new ApprovalData(today.AddDays(-8), "123", 10, false));

            var newFinancialGuarantee = financialGuaranteeCollection.AddFinancialGuarantee(today.AddDays(-5));
            newFinancialGuarantee.Complete(today.AddDays(-4));
            EntityHelper.SetEntityId(newFinancialGuarantee, financialGuaranteeId);

            A.CallTo(() => repository.GetByNotificationId(notificationId)).Returns(financialGuaranteeCollection);

            approval = new FinancialGuaranteeApproval(repository);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task SetsStatusToApproved()
        {
            await approval.Approve(notificationId, financialGuaranteeId, new ApprovalData(today, "456", 5, false));

            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(financialGuaranteeId);

            Assert.Equal(FinancialGuaranteeStatus.Approved, financialGuarantee.Status);
        }

        [Fact]
        public async Task SameReferenceNumber_OldFinancialGuaranteeIsSuperseded()
        {
            await approval.Approve(notificationId, financialGuaranteeId, new ApprovalData(today, "123", 5, false));

            var oldFinancialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(approvedFinancialGuaranteeId);

            Assert.Equal(FinancialGuaranteeStatus.Superseded, oldFinancialGuarantee.Status);
        }

        [Fact]
        public async Task NewReferenceNumber_OldFinancialGuaranteeIsReleased()
        {
            await approval.Approve(notificationId, financialGuaranteeId, new ApprovalData(today, "456", 5, false));

            var oldFinancialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(approvedFinancialGuaranteeId);

            Assert.Equal(FinancialGuaranteeStatus.Released, oldFinancialGuarantee.Status);
        }

        [Fact]
        public async Task ReleaseDateIsSameAsDecisionDate()
        {
            await approval.Approve(notificationId, financialGuaranteeId, new ApprovalData(today, "456", 5, false));

            var oldFinancialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(approvedFinancialGuaranteeId);

            Assert.Equal(today, oldFinancialGuarantee.ReleasedDate.Value);
        }
    }
}
