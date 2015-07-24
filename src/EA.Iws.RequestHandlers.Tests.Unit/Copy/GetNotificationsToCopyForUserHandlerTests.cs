namespace EA.Iws.RequestHandlers.Tests.Unit.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.WasteType;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Helpers;
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
        private static readonly Guid Notification3Id = new Guid("714E80A3-1EEC-4596-90C9-BD0523F10E63");
        private static readonly Guid DestinationNotificationId = new Guid("1F515AE5-02C3-45D4-BFF9-2497B8B361F8");
        private static readonly Guid DestinationNotificationId2 = new Guid("0171210F-E5D1-46E1-81DC-E5FEF1016D96");

        private readonly GetNotificationsToCopyForUserHandler handler;
        private readonly TestUserContext userContext;
        private readonly IwsContext context;

        public GetNotificationsToCopyForUserHandlerTests()
        {
            var helper = new DbContextHelper();
            userContext = new TestUserContext(UserWithNotificationsId);

            context = A.Fake<IwsContext>(options => options.WithArgumentsForConstructor(() => new IwsContext(userContext)));

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

            var notification3 = new NotificationApplication(UserWithNotificationsId, NotificationType.Disposal,
                UKCompetentAuthority.England, 1);
            EntityHelper.SetEntityId(notification3, Notification3Id);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.Exporter, ExporterFactory.Create(new Guid("4FF00D5F-E3CD-4F2E-9059-4E7A60ED463E")), notification3);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.Importer, ImporterFactory.Create(new Guid("DA0C2B9A-3370-4265-BA0D-2F7030241E7C")), notification3);
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.WasteType, WasteType.CreateOtherWasteType("wood"), notification3);

            var destinationNotification = new NotificationApplication(UserWithNotificationsId, NotificationType.Recovery,
                UKCompetentAuthority.England, 1);
            EntityHelper.SetEntityId(destinationNotification, DestinationNotificationId);

            var destinationNotification2 = new NotificationApplication(UserWithoutNotificationsId, NotificationType.Recovery,
                UKCompetentAuthority.England, 1);
            EntityHelper.SetEntityId(destinationNotification2, DestinationNotificationId2);

            A.CallTo(() => context.NotificationApplications).Returns(helper.GetAsyncEnabledDbSet(new[]
            {
                notification1,
                notification2,
                notification3,
                destinationNotification,
                destinationNotification2
            }));

            handler = new GetNotificationsToCopyForUserHandler(context, userContext);
        }

        [Fact]
        public async Task UserHasNoNotification_ReturnsEmptyList()
        {
            var request = new GetNotificationsToCopyForUser(DestinationNotificationId2);
            userContext.ReturnsId = UserWithoutNotificationsId;

            var result = await handler.HandleAsync(request);

            Assert.Empty(result);
        }

        [Fact]
        public async Task UserHasNotifications_ReturnsNotificationsOfSameType()
        {
            var request = new GetNotificationsToCopyForUser(DestinationNotificationId);
            userContext.ReturnsId = UserWithNotificationsId;

            var result = await handler.HandleAsync(request);

            Assert.Equal(2, result.Count);
        }
    }
}
