namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.UserAdministration
{
    using System;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Core.Admin;
    using Core.Admin.UserAdministration;
    using Core.Authorization;
    using DataAccess;
    using DataAccess.Identity;
    using FakeItEasy;
    using Microsoft.AspNet.Identity;
    using RequestHandlers.Admin.UserAdministration;
    using Requests.Admin.UserAdministration;
    using Xunit;

    public class SetUserApprovalsHandlerTests
    {
        private readonly SetUserApprovals approvePendingAdminMessage;
        private readonly IwsContext context;
        private readonly Func<Guid, IwsContext, InternalUserStatus?> getUserStatusFromContext;
        private readonly SetUserApprovalsHandler handler;
        private readonly TestUserContext userContext;

        public SetUserApprovalsHandlerTests()
        {
            userContext = new TestUserContext(Guid.Empty);
            context = new TestIwsContext(userContext);

            context.InternalUsers.AddRange(new InternalUserCollection().Users);
            var userManager = A.Fake<UserManager<ApplicationUser>>();

            handler = new SetUserApprovalsHandler(context, userContext, userManager);

            approvePendingAdminMessage = new SetUserApprovals(new[]
            {
                new UserApproval(InternalUserCollection.AdminPendingId,
                    ApprovalAction.Approve, UserRole.Administrator)
            });

            getUserStatusFromContext =
                (id, ctxt) => { return ctxt.InternalUsers.Single(u => u.UserId == id.ToString()).Status; };
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
            Assert.Equal(InternalUserStatus.Approved,
                getUserStatusFromContext(InternalUserCollection.AdminPendingUserId, context));
        }

        [Fact]
        public async Task AdminCanRejectUser()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedUserId;

            var result = await handler.HandleAsync(new SetUserApprovals(new[]
            {
                new UserApproval(InternalUserCollection.AdminPendingId, ApprovalAction.Reject, null)
            }));

            Assert.True(result);
            Assert.Equal(InternalUserStatus.Rejected,
                getUserStatusFromContext(InternalUserCollection.AdminPendingUserId, context));
        }

        [Fact]
        public async Task RequestTargetsMissingUser_ReturnsFalseAndDoesNotSave()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedUserId;

            var message = new SetUserApprovals(new[]
            {
                new UserApproval(Guid.Empty, ApprovalAction.Approve, UserRole.Administrator),
                new UserApproval(InternalUserCollection.AdminPendingId,
                    ApprovalAction.Approve, UserRole.Administrator)
            });

            var result = await handler.HandleAsync(message);

            Assert.False(result);
            Assert.Equal(InternalUserStatus.Pending,
                getUserStatusFromContext(InternalUserCollection.AdminPendingUserId, context));
        }
    }
}