namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using FakeItEasy;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using Xunit;

    public class RefuseFinancialGuaranteeHandlerTests : FinancialGuaranteeDecisionTests
    {
        private readonly RefuseFinancialGuaranteeHandler handler;
        private readonly TestFinancialGuarantee financialGuarantee;
        private readonly RefuseFinancialGuarantee refuseFinancialGuarantee =
            new RefuseFinancialGuarantee(ApplicationCompletedId, FirstDate, "test");

        public RefuseFinancialGuaranteeHandlerTests()
        {
            context = new TestIwsContext();

            financialGuarantee = new TestFinancialGuarantee { NotificationApplicationId = ApplicationCompletedId };

            context.FinancialGuarantees.Add(financialGuarantee);

            handler = new RefuseFinancialGuaranteeHandler(context);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new RefuseFinancialGuarantee(Guid.Empty, FirstDate, "test")));
        }

        [Fact]
        public async Task Saves()
        {
            await
                handler.HandleAsync(refuseFinancialGuarantee);

            Assert.Equal(1, ((TestIwsContext)context).SaveChangesCount);
        }

        [Fact]
        public async Task CallsRefuse()
        {
            await
                handler.HandleAsync(refuseFinancialGuarantee);

            Assert.True(financialGuarantee.RefuseCalled);
        }

        [Fact]
        public async Task RefuseThrows_Propagates()
        {
            financialGuarantee.RejectThrows = true;

            await
                Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(refuseFinancialGuarantee));
        }

        [Fact]
        public async Task ReturnsTrueByDefault()
        {
            var result = await handler.HandleAsync(refuseFinancialGuarantee);

            Assert.True(result);
        }
    }
}
