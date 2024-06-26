﻿namespace EA.Iws.RequestHandlers.Tests.Unit.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Core.WasteType;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Exporter;
    using RequestHandlers.Copy;
    using Requests.Copy;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

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
            userContext = new TestUserContext(UserWithNotificationsId);

            context = new TestIwsContext(userContext);

            var notification1 = NotificationApplicationFactory.Create(UserWithNotificationsId, NotificationType.Recovery,
                UKCompetentAuthority.England, 1);
            EntityHelper.SetEntityId(notification1, Notification1Id);
            var importer1 = ImporterFactory.Create(Notification1Id, new Guid("DA0C2B9A-3370-4265-BA0D-2F7030241E7C"));
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.WasteType, WasteType.CreateOtherWasteType("wood", WasteCategoryType.Oils), notification1);

            var notification2 = NotificationApplicationFactory.Create(UserWithNotificationsId, NotificationType.Recovery,
                UKCompetentAuthority.England, 2);
            EntityHelper.SetEntityId(notification2, Notification2Id);
            var importer2 = ImporterFactory.Create(Notification2Id, new Guid("CD8FE7F5-B0EF-47E4-A198-D1E531A6CCDF"));
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.WasteType, WasteType.CreateRdfWasteType(new List<WasteAdditionalInformation>
            {
                WasteAdditionalInformation.CreateWasteAdditionalInformation("toffee", 1, 10, WasteInformationType.AshContent)
            }), notification2);

            var notification3 = NotificationApplicationFactory.Create(UserWithNotificationsId, NotificationType.Disposal,
                UKCompetentAuthority.England, 1);
            EntityHelper.SetEntityId(notification3, Notification3Id);
            var importer3 = ImporterFactory.Create(Notification3Id, new Guid("AF7ADA0A-E81B-4A7F-9837-52591B219DD3"));
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.WasteType, WasteType.CreateOtherWasteType("wood", WasteCategoryType.Oils), notification3);

            var destinationNotification = NotificationApplicationFactory.Create(UserWithNotificationsId, NotificationType.Recovery,
                UKCompetentAuthority.England, 1);
            EntityHelper.SetEntityId(destinationNotification, DestinationNotificationId);

            var destinationNotification2 = NotificationApplicationFactory.Create(UserWithoutNotificationsId, NotificationType.Recovery,
                UKCompetentAuthority.England, 1);
            EntityHelper.SetEntityId(destinationNotification2, DestinationNotificationId2);

            context.NotificationApplications.AddRange(new[]
            {
                notification1,
                notification2,
                notification3,
                destinationNotification,
                destinationNotification2
            });

            context.Exporters.AddRange(new[]
            {
                CreateExporter(Notification1Id),
                CreateExporter(Notification2Id),
                CreateExporter(Notification3Id)
            });

            context.Importers.AddRange(new[]
            {
                importer1,
                importer2,
                importer3
            });

            handler = new GetNotificationsToCopyForUserHandler(context, userContext);
        }

        private Exporter CreateExporter(Guid notificationId)
        {
            return new TestableExporter
            {
                NotificationId = notificationId,
                Address = TestableAddress.WitneyAddress,
                Business = TestableBusiness.WasteSolutions,
                Contact = TestableContact.MikeMerry
            };
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