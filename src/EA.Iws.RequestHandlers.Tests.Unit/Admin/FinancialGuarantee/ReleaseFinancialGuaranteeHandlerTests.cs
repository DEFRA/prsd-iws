namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using Xunit;

    public class ReleaseFinancialGuaranteeHandlerTests : FinancialGuaranteeDecisionTests
    {
        private readonly ReleaseFinancialGuaranteeHandler handler;
        private readonly TestFinancialGuarantee financialGuarantee;
        private readonly ReleaseFinancialGuarantee releaseFinancialGuarantee =
            new ReleaseFinancialGuarantee(ApplicationCompletedId, FinancialGuaranteeId, FirstDate);

        private readonly IFinancialGuaranteeRepository repository;

        public ReleaseFinancialGuaranteeHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<IFinancialGuaranteeRepository>();

            var financialGuaranteeCollection = new TestFinancialGuaranteeCollection(ApplicationCompletedId);
            financialGuarantee = new TestFinancialGuarantee(FinancialGuaranteeId);
            financialGuaranteeCollection.AddExistingFinancialGuarantee(financialGuarantee);

            A.CallTo(() => repository.GetByNotificationId(ApplicationCompletedId)).Returns(financialGuaranteeCollection);

            handler = new ReleaseFinancialGuaranteeHandler(repository, context);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new ReleaseFinancialGuarantee(Guid.Empty, Guid.Empty, FirstDate)));
        }

        [Fact]
        public async Task Saves()
        {
            await handler.HandleAsync(releaseFinancialGuarantee);

            Assert.Equal(1, ((TestIwsContext)context).SaveChangesCount);
        }

        [Fact]
        public async Task CallsRelease()
        {
            await handler.HandleAsync(releaseFinancialGuarantee);

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
        public async Task StatusReleased()
        {
            await handler.HandleAsync(releaseFinancialGuarantee);
            
            Assert.Equal(FinancialGuaranteeStatus.Released, financialGuarantee.Status);
        }
    }
}
