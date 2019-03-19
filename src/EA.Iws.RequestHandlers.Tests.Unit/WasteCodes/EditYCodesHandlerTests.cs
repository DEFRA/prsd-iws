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

    public class EditYCodesHandlerTests : WasteCodeHandlerTests
    {
        private static readonly Guid NotificationId = new Guid("6D316E99-D184-4EB2-965B-FB58D03883A7");

        private readonly EditYCodesHandler handler;
        private readonly TestIwsContext context;
        private readonly IList<WasteCodeInfo> codes;
        private readonly Func<TestIwsContext, NotificationApplication> getTestNotification
            = ctxt => ctxt.NotificationApplications.Single(na => na.Id == NotificationId);

        public EditYCodesHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(Guid.Empty));

            handler = new EditYCodesHandler(context);

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
        public async Task EditYCodes_NotApplicableRemovesAllCodesWithSingle()
        {
            await handler.HandleAsync(new EditYCodes(NotificationId, null, true));

            Assert.Single(getTestNotification(context).YCodes, yc => yc.IsNotApplicable);
        }

        [Fact]
        public async Task EditYCodes_SetsYCodeForNotification()
        {
            var codes = new Guid[]
            {
                new Guid("060033CD-7C75-4A6A-9C8C-ABAB14C35FC2")
            };

            await handler.HandleAsync(new EditYCodes(NotificationId, codes, false));

            Assert.NotNull(getTestNotification(context).YCodes);
        }

        [Fact]
        public async Task EditYCodes_SetMultipleYCodesForNotification()
        {
            var codes = new Guid[]
            {
                new Guid("060033CD-7C75-4A6A-9C8C-ABAB14C35FC2"),
                new Guid("AEC9B57A-9D19-4262-8069-4D887E6B2C8C")
            };

            await handler.HandleAsync(new EditYCodes(NotificationId, codes, false));

            Assert.NotNull(getTestNotification(context).YCodes);
            Assert.True(getTestNotification(context).YCodes.Count() == codes.Count());
        }

        [Fact]
        public async Task EditYCodes_SaveChangesIsCalled()
        {
            var codes = new Guid[]
            {
                new Guid("060033CD-7C75-4A6A-9C8C-ABAB14C35FC2")
            };

            await handler.HandleAsync(new EditYCodes(NotificationId, codes, false));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task EditYCodes_ReturnsTrue()
        {
            var codes = new Guid[]
            {
                new Guid("060033CD-7C75-4A6A-9C8C-ABAB14C35FC2")
            };

            var result = await handler.HandleAsync(new EditYCodes(NotificationId, codes, false));

            Assert.True(result);
        }
    }
}
