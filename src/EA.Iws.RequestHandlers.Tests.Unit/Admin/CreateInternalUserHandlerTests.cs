namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Linq;
    using Core.Notification;
    using RequestHandlers.Admin;
    using Requests.Admin;
    using Xunit;

    public class CreateInternalUserHandlerTests
    {
        private readonly TestIwsContext context;
        private readonly CreateInternalUserHandler handler;
        private readonly string userId = "0F7E4119-FE8B-431D-BF4C-A5373C14C460";
        private readonly Guid localAreaId = new Guid("CB91C54F-498C-4D99-A4B3-71FB3EEBACFA");

        public CreateInternalUserHandlerTests()
        {
            context = new TestIwsContext();
            handler = new CreateInternalUserHandler(context);
        }

        [Fact]
        public async void AddsNewIntneralUser()
        {
            var message = new CreateInternalUser(userId, "job title",
                localAreaId, CompetentAuthority.England);

            await handler.HandleAsync(message);

            Assert.Equal(1, context.InternalUsers.Count());
        }

        [Fact]
        public async void SaveChangesIsCalled()
        {
            var message = new CreateInternalUser(userId, "job title",
                localAreaId, CompetentAuthority.England);

            await handler.HandleAsync(message);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}