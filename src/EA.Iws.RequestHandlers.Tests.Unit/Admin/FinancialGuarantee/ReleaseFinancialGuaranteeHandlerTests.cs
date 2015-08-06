namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using Helpers;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using Xunit;

    public class ReleaseFinancialGuaranteeHandlerTests : FinancialGuaranteeDecisionTests
    {
        private readonly ReleaseFinancialGuaranteeHandler handler;
        private readonly TestFinancialGuarantee financialGuarantee;
        private readonly ReleaseFinancialGuarantee releaseFinancialGuarantee =
            new ReleaseFinancialGuarantee(ApplicationCompletedId, FirstDate);

        public ReleaseFinancialGuaranteeHandlerTests()
        {
            context = A.Fake<IwsContext>();
            var helper = new DbContextHelper();

            financialGuarantee = new TestFinancialGuarantee { NotificationApplicationId = ApplicationCompletedId };

            A.CallTo(() => context.FinancialGuarantees).Returns(helper.GetAsyncEnabledDbSet(new List<FinancialGuarantee>
            {
                financialGuarantee
            }));

            handler = new ReleaseFinancialGuaranteeHandler(context);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new ReleaseFinancialGuarantee(Guid.Empty, FirstDate)));
        }

        [Fact]
        public async Task Saves()
        {
            await
                handler.HandleAsync(releaseFinancialGuarantee);

            A.CallTo(() => context.SaveChangesAsync()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task CallsRelease()
        {
            await
                handler.HandleAsync(releaseFinancialGuarantee);

            Assert.True(financialGuarantee.ReleaseCalled);
        }

        [Fact]
        public async Task ReleaseThrows_Propagates()
        {
            financialGuarantee.RefuseThrows = true;

            await
                Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(releaseFinancialGuarantee));
        }

        [Fact]
        public async Task ReturnsTrueByDefault()
        {
            var result = await handler.HandleAsync(releaseFinancialGuarantee);

            Assert.True(result);
        }

        [Fact]
        public async Task StatusReleased()
        {
            var result = await handler.HandleAsync(releaseFinancialGuarantee);
            
            Assert.Equal(FinancialGuaranteeStatus.Released, financialGuarantee.Status);
        }
    }
}
