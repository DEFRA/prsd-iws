namespace EA.Iws.RequestHandlers.Tests.Unit.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using RequestHandlers.WasteCodes;
    using Requests.WasteCodes;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetBaselOecdCodeForNotificationHandlerTests : WasteCodeHandlerTests
    {
        private static readonly Guid NotificationId = new Guid("6D316E99-D184-4EB2-965B-FB58D03883A7");

        private readonly SetBaselOecdCodeForNotificationHandler handler;
        private readonly TestIwsContext context;
        private readonly IList<WasteCodeInfo> codes;
        private readonly Func<TestIwsContext, NotificationApplication> getTestNotification
            = ctxt => ctxt.NotificationApplications.Single(na => na.Id == NotificationId);

        public SetBaselOecdCodeForNotificationHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(Guid.Empty));

            handler = new SetBaselOecdCodeForNotificationHandler(context);

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
        public async Task SetBaselCodeWhereNoCodeExists()
        {
            await
                handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Basel, false,
                    FirstBaselCode.Id));

            Assert.Single(getTestNotification(context).WasteCodes, wc => wc.CodeType == CodeType.Basel && wc.WasteCode.Id == FirstBaselCode.Id);
        }

        [Fact]
        public async Task SetNotApplicableBaselCode()
        {
            Assert.Null(getTestNotification(context).BaselOecdCode);

            await handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Basel, true, null));

            Assert.NotNull(getTestNotification(context).BaselOecdCode);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await
                handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Basel, false,
                    FirstBaselCode.Id));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task RemovesPreviousBaselCode()
        {
            codes.Add(new TestableWasteCodeInfo(SecondBaselCode));

            await
                handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Basel, false,
                    FirstBaselCode.Id));

            Assert.Equal(FirstBaselCode.Id, getTestNotification(context).BaselOecdCode.WasteCode.Id);
        }

        [Fact]
        public async Task SetOecdCodeWhereNoCodeExists()
        {
            await
                handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Oecd, false,
                    FirstOecdCode.Id));

            Assert.Equal(FirstOecdCode.Id, getTestNotification(context).BaselOecdCode.WasteCode.Id);
            Assert.Equal(CodeType.Oecd, getTestNotification(context).BaselOecdCode.CodeType);
        }

        [Fact]
        public async Task SetOecdCodeRemovesPreviousBaselCode()
        {
            codes.Add(new TestableWasteCodeInfo(FirstBaselCode));

            await handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Oecd, false,
                FirstOecdCode.Id));

            Assert.Empty(getTestNotification(context).WasteCodes.Where(wc => wc.CodeType == CodeType.Basel));
            Assert.Equal(FirstOecdCode.Id, getTestNotification(context).BaselOecdCode.WasteCode.Id);
        }

        [Fact]
        public async Task NotificationDoesNotExistThrows()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new SetBaselOecdCodeForNotification(Guid.Empty, CodeType.Basel, false,
                            Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsTrue()
        {
            var result =
                await
                    handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Basel, false,
                        FirstBaselCode.Id));

            Assert.True(result);
        }

        [Fact]
        public async Task CannotSetCodeWithWrongTypeSpecified()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new SetBaselOecdCodeForNotification(
                        NotificationId, CodeType.Basel, false, FirstOecdCode.Id)));
        }

        [Fact]
        public async Task SetNotApplicableRemovesPreviousCodeSameCodeType()
        {
            codes.Add(new TestableWasteCodeInfo(FirstBaselCode));

            await handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Basel, true, null));

            Assert.Null(getTestNotification(context).BaselOecdCode.WasteCode);
            Assert.True(getTestNotification(context).BaselOecdCode.IsNotApplicable);
            Assert.Equal(CodeType.Basel, getTestNotification(context).BaselOecdCode.CodeType);
        }

        [Fact]
        public void CannotCreateARequestNotApplicableFalseWithNullCode()
        {
            Assert.Throws<InvalidOperationException>(
                () => new SetBaselOecdCodeForNotification(NotificationId, CodeType.Basel, false, null));
        }

        [Fact]
        public async Task SetNotApplicableRemovesPreviousCodeOtherCodeType()
        {
            codes.Add(new TestableWasteCodeInfo(FirstBaselCode));

            await handler.HandleAsync(new SetBaselOecdCodeForNotification(NotificationId, CodeType.Oecd, true, null));

            Assert.Null(getTestNotification(context).BaselOecdCode.WasteCode);
            Assert.True(getTestNotification(context).BaselOecdCode.IsNotApplicable);
            Assert.Equal(CodeType.Oecd, getTestNotification(context).BaselOecdCode.CodeType);
        }

        [Fact]
        public async Task CannotSetNotApplicableWithWrongCodeType()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new SetBaselOecdCodeForNotification(
                NotificationId,
                CodeType.Ewc,
                false,
                new Guid("CEBFDD35-6B19-4583-8DB1-C782F1675050"))));
        }

        [Fact]
        public void RequestWithNotApplicableTrueMustHaveNullCodeId()
        {
            Assert.Throws<InvalidOperationException>(() => new SetBaselOecdCodeForNotification(
                NotificationId,
                CodeType.Basel,
                true,
                Guid.Empty));
        }
    }
}
