namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using Xunit;

    public class ApproveFinancialGuaranteeHandlerTests : FinancialGuaranteeDecisionTests
    {
        private const int AnyInt = 7;
        private const string BlanketBondReference = "ref 23";
        private readonly ApproveFinancialGuaranteeHandler handler;
        private readonly TestFinancialGuarantee financialGuarantee;
        private readonly ApproveFinancialGuarantee approveFinancialGuarantee =
            new ApproveFinancialGuarantee(ApplicationCompletedId, FinancialGuaranteeId, FirstDate, BlanketBondReference, AnyInt, true);

        private readonly IFinancialGuaranteeRepository repository;

        public ApproveFinancialGuaranteeHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<IFinancialGuaranteeRepository>();

            var financialGuaranteeCollection = new TestFinancialGuaranteeCollection(ApplicationCompletedId);
            financialGuarantee = new TestFinancialGuarantee(FinancialGuaranteeId);
            financialGuaranteeCollection.AddExistingFinancialGuarantee(financialGuarantee);

            A.CallTo(() => repository.GetByNotificationId(ApplicationCompletedId)).Returns(financialGuaranteeCollection);

            handler = new ApproveFinancialGuaranteeHandler(repository, context);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new ApproveFinancialGuarantee(Guid.Empty, FinancialGuaranteeId, FirstDate, BlanketBondReference, AnyInt, true)));
        }

        [Fact]
        public async Task NotificationDoesNotExist_DoesNotCallApprove()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new ApproveFinancialGuarantee(Guid.Empty, FinancialGuaranteeId, FirstDate, BlanketBondReference, AnyInt, true)));

            Assert.False(financialGuarantee.ApproveCalled);
        }

        [Fact]
        public async Task Saves()
        {
            await
                handler.HandleAsync(approveFinancialGuarantee);

            Assert.Equal(1, ((TestIwsContext)context).SaveChangesCount);
        }

        [Fact]
        public async Task CallsApprove()
        {
            await
                handler.HandleAsync(approveFinancialGuarantee);

            Assert.True(financialGuarantee.ApproveCalled);
        }

        [Fact]
        public async Task ApproveThrows_Propagates()
        {
            financialGuarantee.ApproveThrows = true;

            await
                Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(approveFinancialGuarantee));
        }
    }
}
