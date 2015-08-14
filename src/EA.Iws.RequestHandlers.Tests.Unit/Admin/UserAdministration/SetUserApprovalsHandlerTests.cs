namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using RequestHandlers.Admin.UserAdministration;
    using Requests.Admin.UserAdministration;
    using Xunit;

    public class SetUserApprovalsHandlerTests
    {
        private readonly SetUserApprovalsHandler handler;
        private readonly IwsContext context;
        private readonly TestUserContext userContext;
        private readonly SetUserApprovals approvePendingAdminMessage;
        private readonly Func<Guid, IwsContext, InternalUserStatus?> getUserStatusFromContext;

        public SetUserApprovalsHandlerTests()
        {
            this.userContext = new TestUserContext(Guid.Empty);
            this.context = new TestIwsContext(userContext);

            context.InternalUsers.AddRange(new InternalUserCollection().Users);

            handler = new SetUserApprovalsHandler(context, userContext);

            approvePendingAdminMessage = new SetUserApprovals(new[]
            {
                new KeyValuePair<Guid, ApprovalAction>(InternalUserCollection.AdminPendingId,
                    ApprovalAction.Approve)
            });

            getUserStatusFromContext = (id, ctxt) => { return ctxt.InternalUsers.Single(u => u.UserId == id.ToString()).Status; };
        }

        [Fact]
        public async Task SetAsNonAdminUser_ThrowsSecurityException()
        {
            userContext.ReturnsId = InternalUserCollection.NonAdminUserId;

            await Assert.ThrowsAsync<SecurityException>(() => handler.HandleAsync(approvePendingAdminMessage));
        }

        [Fact]
        public async Task SetAsPendingAdminUser_ThrowsSecurityException()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminPendingUserId;

            await Assert.ThrowsAsync<SecurityException>(() => handler.HandleAsync(approvePendingAdminMessage));
        }

        [Fact]
        public async Task AdminCanApproveUser()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedUserId;

            var result = await handler.HandleAsync(approvePendingAdminMessage);

            Assert.True(result);
            Assert.Equal(InternalUserStatus.Approved, getUserStatusFromContext(InternalUserCollection.AdminPendingUserId, context));
        }

        [Fact]
        public async Task AdminCanRejectUser()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedUserId;

            var result = await handler.HandleAsync(new SetUserApprovals(new[]
            {
                new KeyValuePair<Guid, ApprovalAction>(InternalUserCollection.AdminPendingId, ApprovalAction.Reject) 
            }));

            Assert.True(result);
            Assert.Equal(InternalUserStatus.Rejected, getUserStatusFromContext(InternalUserCollection.AdminPendingUserId, context));
        }

        [Fact]
        public async Task RequestTargetsMissingUser_ReturnsFalseAndDoesNotSave()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedUserId;

            var message = new SetUserApprovals(new[]
            {
                new KeyValuePair<Guid, ApprovalAction>(Guid.Empty, ApprovalAction.Approve),
                new KeyValuePair<Guid, ApprovalAction>(InternalUserCollection.AdminPendingId,
                    ApprovalAction.Approve)
            });

            var result = await handler.HandleAsync(message);

            Assert.False(result);
            Assert.Equal(InternalUserStatus.Pending, getUserStatusFromContext(InternalUserCollection.AdminPendingUserId, context));
        }
    }
}
