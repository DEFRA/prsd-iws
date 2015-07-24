namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using FakeItEasy;
    using RequestHandlers.Admin.UserAdministration;
    using Requests.Admin.UserAdministration;
    using TestHelpers;
    using Xunit;

    public class SetUserApprovalsHandlerTests
    {
        private readonly SetUserApprovalsHandler handler;
        private readonly IwsContext context;
        private readonly TestUserContext userContext;
        private readonly DbContextHelper contextHelper = new DbContextHelper();
        private readonly SetUserApprovals approvePendingAdminMessage;
        private readonly Func<Guid, IwsContext, InternalUserStatus?> getUserStatusFromContext;

        public SetUserApprovalsHandlerTests()
        {
            this.context = A.Fake<IwsContext>();
            A.CallTo(() => context.Users).Returns(contextHelper.GetAsyncEnabledDbSet(new InternalUserCollection().Users));

            this.userContext = new TestUserContext(Guid.Empty);

            handler = new SetUserApprovalsHandler(context, userContext, new TestMessageService());

            approvePendingAdminMessage = new SetUserApprovals(new[]
            {
                new KeyValuePair<string, ApprovalAction>(InternalUserCollection.AdminPendingId.ToString(),
                    ApprovalAction.Approve)
            });

            getUserStatusFromContext = (id, ctxt) => { return ctxt.Users.Single(u => u.Id == id.ToString()).InternalUserStatus; };
        }

        [Fact]
        public async Task SetAsNonAdminUser_ReturnsFalse()
        {
            userContext.ReturnsId = InternalUserCollection.NonAdminUserId;

            var result = await handler.HandleAsync(approvePendingAdminMessage);

            Assert.False(result);
            Assert.Equal(InternalUserStatus.Pending, getUserStatusFromContext(InternalUserCollection.AdminPendingId, context));
        }

        [Fact]
        public async Task SetAsPendingAdminUser_ReturnsFalse()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminPendingId;

            var result = await handler.HandleAsync(approvePendingAdminMessage);

            Assert.False(result);
        }

        [Fact]
        public async Task AdminCanApproveUser()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedId;

            var result = await handler.HandleAsync(approvePendingAdminMessage);

            Assert.True(result);
            Assert.Equal(InternalUserStatus.Approved, getUserStatusFromContext(InternalUserCollection.AdminPendingId, context));
        }

        [Fact]
        public async Task AdminCanRejectUser()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedId;

            var result = await handler.HandleAsync(new SetUserApprovals(new[]
            {
                new KeyValuePair<string, ApprovalAction>(InternalUserCollection.AdminPendingId.ToString(), ApprovalAction.Reject) 
            }));

            Assert.True(result);
            Assert.Equal(InternalUserStatus.Rejected, getUserStatusFromContext(InternalUserCollection.AdminPendingId, context));
        }

        [Fact]
        public async Task RequestTargetsMissingUser_ReturnsFalseAndDoesNotSave()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedId;

            var message = new SetUserApprovals(new[]
            {
                new KeyValuePair<string, ApprovalAction>(Guid.Empty.ToString(), ApprovalAction.Approve),
                new KeyValuePair<string, ApprovalAction>(InternalUserCollection.AdminPendingId.ToString(),
                    ApprovalAction.Approve)
            });

            var result = await handler.HandleAsync(message);

            Assert.False(result);
            Assert.Equal(InternalUserStatus.Pending, getUserStatusFromContext(InternalUserCollection.AdminPendingId, context));
        }
    }
}
