namespace EA.Iws.RequestHandlers.Tests.Unit.MeansOfTransport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Core.MeansOfTransport;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using RequestHandlers.MeansOfTransport;
    using Requests.MeansOfTransport;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetMeansOfTransportForNotificationHandlerTests
    {
        private readonly Guid notificationNoMeansOfTransportId = new Guid("59031296-8D25-4AB5-A986-2A08DCC64844");
        private readonly Guid notificationNotExistingGuid = new Guid("70266E02-AE73-4531-B3BF-8DFDF3666CB5");
        private readonly Guid notificationWithMeansOfTransportId = new Guid("66DE0B39-E9C0-4ED8-B02C-64AA25F7612A");
        private readonly string meansOfTransportValue = "R;S;R";

        private readonly IwsContext context;
        private readonly SetMeansOfTransportForNotificationHandler handler;

        public SetMeansOfTransportForNotificationHandlerTests()
        {
            this.context = A.Fake<IwsContext>();
            var dbSetHelper = new DbContextHelper();
            var notificationNoMeans = new NotificationApplication(Guid.Empty, NotificationType.Disposal,
                UKCompetentAuthority.England, 500);
            var notificationMeans = new NotificationApplication(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            EntityHelper.SetEntityId(notificationMeans, notificationWithMeansOfTransportId);
            EntityHelper.SetEntityId(notificationNoMeans, notificationNoMeansOfTransportId);
            typeof(NotificationApplication).GetProperty("MeansOfTransportInternal",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(notificationMeans, meansOfTransportValue);

            var dbSet = dbSetHelper.GetAsyncEnabledDbSet(new NotificationApplication[]
            {
                notificationNoMeans,
                notificationMeans
            });

            A.CallTo(() => context.NotificationApplications).Returns(dbSet);

            handler = new SetMeansOfTransportForNotificationHandler(context);
        }

        [Fact]
        public async Task Set_ToNotificationWhichDoesNotExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new SetMeansOfTransportForNotification(notificationNotExistingGuid, new List<MeansOfTransport>())));
        }

        [Fact]
        public async Task Set_ToNullMeansOfTransportList_Throws()
        {
            await
                Assert.ThrowsAsync<ArgumentNullException>(
                    () =>
                        handler.HandleAsync(new SetMeansOfTransportForNotification(notificationNoMeansOfTransportId,
                            null)));
        }

        [Fact]
        public async Task Set_ToEmptyMeansOfTransportList_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new SetMeansOfTransportForNotification(notificationNoMeansOfTransportId,
                            new List<MeansOfTransport>())));
        }

        [Fact]
        public async Task Set_ToMeansOfTransport_ReturnsNotificationId()
        {
            var means = new List<MeansOfTransport>
            {
                MeansOfTransport.Air,
                MeansOfTransport.Road
            };

            var result = await handler.HandleAsync(new SetMeansOfTransportForNotification(notificationNoMeansOfTransportId, means));

            Assert.Equal(notificationNoMeansOfTransportId, result);
        }

        [Fact]
        public async Task Set_ToMeansOfTransport_PersistsChangesToContext()
        {
            var means = new[] { MeansOfTransport.Air, MeansOfTransport.Road };

            var result =
                await
                    handler.HandleAsync(new SetMeansOfTransportForNotification(notificationNoMeansOfTransportId, means));

            var notification = context.NotificationApplications.Single(n => n.Id == notificationNoMeansOfTransportId);

            Assert.Equal(means, notification.MeansOfTransport);
        }

        [Fact]
        public async Task Set_ToNotificationWithExistingMeansOfTransport_ReturnsCorrectId()
        {
            var means = new[] { MeansOfTransport.Air, MeansOfTransport.Road };

            var result =
                await
                    handler.HandleAsync(new SetMeansOfTransportForNotification(notificationWithMeansOfTransportId, means));

            Assert.Equal(notificationWithMeansOfTransportId, result);
        }
    }
}
