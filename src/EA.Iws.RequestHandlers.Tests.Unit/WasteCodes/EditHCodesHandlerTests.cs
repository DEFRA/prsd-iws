namespace EA.Iws.RequestHandlers.Tests.Unit.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using RequestHandlers.WasteCodes;
    using Requests.WasteCodes;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class EditHCodesHandlerTests : WasteCodeHandlerTests
    {
        private static readonly Guid NotificationId = new Guid("6D316E99-D184-4EB2-965B-FB58D03883A7");

        private readonly EditHCodesHandler handler;
        private readonly TestIwsContext context;
        private readonly IList<WasteCodeInfo> codes;
        private readonly Func<TestIwsContext, NotificationApplication> getTestNotification
            = ctxt => ctxt.NotificationApplications.Single(na => na.Id == NotificationId);

        public EditHCodesHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(Guid.Empty));

            handler = new EditHCodesHandler(context);

            codes = new List<WasteCodeInfo>();

            context.NotificationApplications.Add(new TestableNotificationApplication
            {
                Id = NotificationId,
                WasteCodes = codes,
                UserId = Guid.Empty
            });

            context.WasteCodes.AddRange(wasteCodes);
        }

        [Fact]
        public async Task EditHCodes_NotApplicableRemovesAllCodesWithSingle()
        {
            await handler.HandleAsync(new EditHCodes(NotificationId, null, true));

            Assert.Single(getTestNotification(context).HCodes, hc => hc.IsNotApplicable);
        }

        [Fact]
        public async Task EditHCodes_SetsHCodeForNotification()
        {
            var codes = new Guid[]
            {
                new Guid("382D0431-A296-48EE-BBDE-1DDD4F5801DC")
            };

            await handler.HandleAsync(new EditHCodes(NotificationId, codes, false));

            Assert.NotNull(getTestNotification(context).HCodes);
        }

        [Fact]
        public async Task EditHCodes_SetMultipleHCodesForNotification()
        {
            var codes = new Guid[]
            {
                new Guid("382D0431-A296-48EE-BBDE-1DDD4F5801DC"),
                new Guid("882B4E36-A6B5-494F-B08F-89E2FDF9A875")
            };

            await handler.HandleAsync(new EditHCodes(NotificationId, codes, false));

            Assert.NotNull(getTestNotification(context).HCodes);
            Assert.True(getTestNotification(context).HCodes.Count() == codes.Count());
        }

        [Fact]
        public async Task EditHCodes_SaveChangesIsCalled()
        {
            var codes = new Guid[]
            {
                new Guid("382D0431-A296-48EE-BBDE-1DDD4F5801DC")
            };

            await handler.HandleAsync(new EditHCodes(NotificationId, codes, false));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task EditHCodes_ReturnsTrue()
        {
            var codes = new Guid[]
            {
                new Guid("382D0431-A296-48EE-BBDE-1DDD4F5801DC")
            };

            var result = await handler.HandleAsync(new EditHCodes(NotificationId, codes, false));

            Assert.True(result);
        }
    }
}
