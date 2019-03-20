﻿namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using Xunit;

    public class ApproveFinancialGuaranteeHandlerTests : FinancialGuaranteeDecisionTests
    {
        private const int AnyInt = 7;
        private const decimal AnyDec = (decimal)12.34;
        private const string BlanketBondReference = "ref 23";
        private readonly ApproveFinancialGuaranteeHandler handler;
        private readonly TestFinancialGuarantee financialGuarantee;
        private readonly ApproveFinancialGuarantee approveFinancialGuarantee =
            new ApproveFinancialGuarantee(ApplicationCompletedId, FinancialGuaranteeId, FirstDate, BlanketBondReference, AnyInt, true, AnyDec, AnyDec);

        private readonly IFinancialGuaranteeRepository repository;

        public ApproveFinancialGuaranteeHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<IFinancialGuaranteeRepository>();

            var financialGuaranteeCollection = new TestFinancialGuaranteeCollection(ApplicationCompletedId);
            financialGuarantee = new TestFinancialGuarantee(FinancialGuaranteeId);
            financialGuarantee.SetStatus(FinancialGuaranteeStatus.ApplicationComplete);
            financialGuarantee.CompletedDate = FirstDate;
            financialGuaranteeCollection.AddExistingFinancialGuarantee(financialGuarantee);

            A.CallTo(() => repository.GetByNotificationId(ApplicationCompletedId)).Returns(financialGuaranteeCollection);

            var approval = new FinancialGuaranteeApproval(repository);

            handler = new ApproveFinancialGuaranteeHandler(approval, context);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new ApproveFinancialGuarantee(Guid.Empty, FinancialGuaranteeId, FirstDate, BlanketBondReference, AnyInt, true, AnyDec, AnyDec)));
        }

        [Fact]
        public async Task NotificationDoesNotExist_DoesNotCallApprove()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new ApproveFinancialGuarantee(Guid.Empty, FinancialGuaranteeId, FirstDate, BlanketBondReference, AnyInt, true, AnyDec, AnyDec)));
        }

        [Fact]
        public async Task Saves()
        {
            await handler.HandleAsync(approveFinancialGuarantee);

            Assert.Equal(1, ((TestIwsContext)context).SaveChangesCount);
        }
    }
}
