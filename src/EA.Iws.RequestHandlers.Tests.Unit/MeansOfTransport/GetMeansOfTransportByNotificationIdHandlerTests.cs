namespace EA.Iws.RequestHandlers.Tests.Unit.MeansOfTransport
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Core.MeansOfTransport;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using RequestHandlers.MeansOfTransport;
    using Requests.MeansOfTransport;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetMeansOfTransportByNotificationIdHandlerTests
    {
        private readonly Guid notificationNoMeansOfTransportId = new Guid("59031296-8D25-4AB5-A986-2A08DCC64844");
        private readonly Guid notificationNotExistingGuid = new Guid("70266E02-AE73-4531-B3BF-8DFDF3666CB5");
        private readonly Guid notificationWithMeansOfTransportId = new Guid("66DE0B39-E9C0-4ED8-B02C-64AA25F7612A");
        private readonly Guid notificationWithAnotherMeansOfTransportId = new Guid("56D8A026-4953-488A-9503-CD5A77A8F983");
        private readonly string meansOfTransportValue = "R;S;R";
        private readonly string anotherMeansOfTransport = "R;A;W;S;R;T;R;A";
        private readonly IwsContext context;
        private readonly GetMeansOfTransportByNotificationIdHandler handler;

        public GetMeansOfTransportByNotificationIdHandlerTests()
        {
            context = new TestIwsContext();
            var notificationNoMeans = new NotificationApplication(TestIwsContext.UserId, NotificationType.Disposal,
                UKCompetentAuthority.England, 500);
            var notificationMeans = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            var anotherNotificationWithMeans = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery,
                UKCompetentAuthority.England, 250);

            EntityHelper.SetEntityId(notificationMeans, notificationWithMeansOfTransportId);
            EntityHelper.SetEntityId(notificationNoMeans, notificationNoMeansOfTransportId);
            EntityHelper.SetEntityId(anotherNotificationWithMeans, notificationWithAnotherMeansOfTransportId);
            typeof(NotificationApplication).GetProperty("MeansOfTransportInternal",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(notificationMeans, meansOfTransportValue);
            typeof(NotificationApplication).GetProperty("MeansOfTransportInternal",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(anotherNotificationWithMeans, anotherMeansOfTransport);

            context.NotificationApplications.AddRange(new[]
            {
                notificationNoMeans,
                notificationMeans,
                anotherNotificationWithMeans
            });

            handler = new GetMeansOfTransportByNotificationIdHandler(context);
        }

        [Fact]
        public async Task NotificationNoMeansOfTransport_ReturnsEmptyList()
        {
            var request = new GetMeansOfTransportByNotificationId(notificationNoMeansOfTransportId);

            var result = await handler.HandleAsync(request);

            Assert.Empty(result);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            var request = new GetMeansOfTransportByNotificationId(notificationNotExistingGuid);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
        }

        [Fact]
        public async Task NotificationWithMeansOfTransport_ReturnsExpectedSequence()
        {
            var request = new GetMeansOfTransportByNotificationId(notificationWithMeansOfTransportId);

            var result = await handler.HandleAsync(request);

            Assert.Equal(new[] { MeansOfTransport.Road, MeansOfTransport.Sea, MeansOfTransport.Road }, result);
        }

        [Fact]
        public async Task NotificationWithAnotherMeansOfTransport_ReturnsExpectedSequence()
        {
            var request = new GetMeansOfTransportByNotificationId(notificationWithAnotherMeansOfTransportId);

            var result = await handler.HandleAsync(request);

            Assert.Equal(new[] 
            { 
                MeansOfTransport.Road, 
                MeansOfTransport.Air, 
                MeansOfTransport.InlandWaterways, 
                MeansOfTransport.Sea,
                MeansOfTransport.Road, 
                MeansOfTransport.Train,
                MeansOfTransport.Road, 
                MeansOfTransport.Air
            }, result);
        }
    }
}
