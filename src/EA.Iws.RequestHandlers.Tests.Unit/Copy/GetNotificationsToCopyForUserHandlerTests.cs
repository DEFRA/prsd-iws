namespace EA.Iws.RequestHandlers.Tests.Unit.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.WasteType;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using RequestHandlers.Copy;
    using Requests.Copy;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetNotificationsToCopyForUserHandlerTests
    {
        private static readonly Guid UserWithNotificationsId = new Guid("A5BC3C9D-86BA-4BB0-86E2-DC94B33F685C");
        private static readonly Guid UserWithoutNotificationsId = new Guid("80E910E1-DC99-4485-B92D-9B9631A8462B");
        private static readonly Guid Notification1Id = new Guid("BCCF3E83-41D0-4204-96F1-B81277239ACD");
        private static readonly Guid Notification2Id = new Guid("9CD6EC5C-1035-46FF-881B-D21B0E33F0DA");

        private readonly GetNotificationsToCopyForUserHandler handler;
        private readonly TestUserContext userContext;
        private readonly IwsContext context;
        private readonly GetNotificationsToCopyForUser request;

        public GetNotificationsToCopyForUserHandlerTests()
        {
            var helper = new DbContextHelper();

            context = A.Fake<IwsContext>();

            var notification1 = new NotificationApplication(UserWithNotificationsId, NotificationType.Recovery,
                UKCompetentAuthority.England, 1);
            EntityHelper.SetEntityId(notification1, Notification1Id);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.Exporter, ExporterFactory.Create(new Guid("4FF00D5F-E3CD-4F2E-9059-4E7A60ED463E")), notification1);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.Importer, ImporterFactory.Create(new Guid("DA0C2B9A-3370-4265-BA0D-2F7030241E7C")), notification1);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.WasteType, WasteType.CreateOtherWasteType("wood"), notification1);

            var notification2 = new NotificationApplication(UserWithNotificationsId, NotificationType.Recovery,
                UKCompetentAuthority.England, 2);
            EntityHelper.SetEntityId(notification2, Notification2Id);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.Exporter, ExporterFactory.Create(new Guid("4FF00D5F-E3CD-4F2E-9059-4E7A60ED463E")), notification2);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.Importer, ImporterFactory.Create(new Guid("DA0C2B9A-3370-4265-BA0D-2F7030241E7C")), notification2);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.WasteType, WasteType.CreateRdfWasteType(new List<WasteComposition>
            {
                WasteComposition.CreateWasteComposition("toffee", 1, 10, ChemicalCompositionCategory.Food)
            }), notification2);

            A.CallTo(() => context.NotificationApplications).Returns(helper.GetAsyncEnabledDbSet(new[]
            {
                notification1,
                notification2
            }));

            userContext = new TestUserContext();

            this.handler = new GetNotificationsToCopyForUserHandler(context, userContext);

            request = new GetNotificationsToCopyForUser();
        }

        [Fact]
        public async Task UserHasNoNotification_ReturnsEmptyList()
        {
            userContext.ReturnsId = UserWithoutNotificationsId;

            var result = await handler.HandleAsync(request);

            Assert.Empty(result);
        }

        [Fact]
        public async Task UserHasNotifications_ReturnsNotifications()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(2, result.Count);
        }

        private class TestUserContext : IUserContext
        {
            public Guid ReturnsId = UserWithNotificationsId;

            public Guid UserId
            {
                get { return ReturnsId; }
            }

            public ClaimsPrincipal Principal
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
