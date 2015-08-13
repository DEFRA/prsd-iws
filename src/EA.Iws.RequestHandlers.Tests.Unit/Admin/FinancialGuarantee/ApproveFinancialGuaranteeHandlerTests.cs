namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using FakeItEasy;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using Xunit;

    public class ApproveFinancialGuaranteeHandlerTests : FinancialGuaranteeDecisionTests
    {
        private const int AnyInt = 7;
        private readonly ApproveFinancialGuaranteeHandler handler;
        private readonly TestFinancialGuarantee financialGuarantee;
        private readonly ApproveFinancialGuarantee approveFinancialGuarantee =
            new ApproveFinancialGuarantee(ApplicationCompletedId, FirstDate, MiddleDate,
                LastDate, AnyInt);

        public ApproveFinancialGuaranteeHandlerTests()
        {
            context = new TestIwsContext();

            financialGuarantee = new TestFinancialGuarantee { NotificationApplicationId = ApplicationCompletedId };

            context.FinancialGuarantees.Add(financialGuarantee);

            handler = new ApproveFinancialGuaranteeHandler(context);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new ApproveFinancialGuarantee(Guid.Empty, FirstDate, MiddleDate, LastDate, AnyInt)));
        }

        [Fact]
        public async Task NotificationDoesNotExist_DoesNotCallApprove()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new ApproveFinancialGuarantee(Guid.Empty, FirstDate, MiddleDate, LastDate, AnyInt)));

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

        [Fact]
        public async Task ReturnsTrueByDefault()
        {
            var result = await handler.HandleAsync(approveFinancialGuarantee);

            Assert.True(result);
        }
    }
}
