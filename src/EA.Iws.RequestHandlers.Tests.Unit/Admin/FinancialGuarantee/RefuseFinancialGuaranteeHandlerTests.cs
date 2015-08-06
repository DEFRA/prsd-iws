namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using Helpers;
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
            context = A.Fake<IwsContext>();
            var helper = new DbContextHelper();

            financialGuarantee = new TestFinancialGuarantee { NotificationApplicationId = ApplicationCompletedId };

            A.CallTo(() => context.FinancialGuarantees).Returns(helper.GetAsyncEnabledDbSet(new List<FinancialGuarantee>
            {
                financialGuarantee
            }));

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

            A.CallTo(() => context.SaveChangesAsync()).MustHaveHappened(Repeated.Exactly.Once);
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
