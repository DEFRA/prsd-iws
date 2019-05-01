namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.TransportRoute;
    using FakeItEasy;
    using RequestHandlers.NotificationAssessment;
    using Requests.NotificationAssessment;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class UpdateTransitStateEntryOrExitHandlerTests
    {
        private readonly UpdateTransitStateEntryOrExitHandler handler;
        private readonly ITransportRouteRepository transportRouteRepository;

        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid transitStateId = Guid.NewGuid();
        private readonly Guid entryPointId = Guid.NewGuid();
        private readonly Guid exitPointId = Guid.NewGuid();
        private readonly Guid countryId = Guid.NewGuid();
        private const string AnyString = "test";
        private const int EntryExitPoints = 2;

        public UpdateTransitStateEntryOrExitHandlerTests()
        {
            transportRouteRepository = A.Fake<ITransportRouteRepository>();

            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));
            context.EntryOrExitPoints.AddRange(new List<EntryOrExitPoint>(EntryExitPoints)
            {
                new TestableEntryOrExitPoint()
                {
                    Id = entryPointId,
                    Country = new TestableCountry() { Id = countryId }
                },
                new TestableEntryOrExitPoint()
                {
                    Id = exitPointId,
                    Country = new TestableCountry() { Id = countryId }
                }
            });

            handler = new UpdateTransitStateEntryOrExitHandler(context, transportRouteRepository);
        }

        [Fact]
        public async Task UpdateTransitStateEntryOrExitHandler_NoTransportRoute_ReturnsEmptyGuid()
        {
            A.CallTo(() => transportRouteRepository.GetByNotificationId(notificationId)).Returns((TransportRoute)null);

            var result = await handler.HandleAsync(GetRequest(entryPointId, exitPointId));

            Assert.Equal(Guid.Empty, result);
        }

        [Fact]
        public async Task UpdateTransitStateEntryOrExitHandler_HasTransportRout_NoTransitState_ReturnsEmptyGuid()
        {
            A.CallTo(() => transportRouteRepository.GetByNotificationId(notificationId))
                .Returns(new TestableTransportRoute() { Id = Guid.NewGuid() });

            var result = await handler.HandleAsync(GetRequest(entryPointId, exitPointId));

            Assert.Equal(Guid.Empty, result);
        }

        [Fact]
        public async Task UpdateTransitStateEntryOrExitHandler_CallsUpdateToTransitState_ReturnsTransitStateId()
        {
            var transitState = new TestableTransitState()
            {
                Id = transitStateId,
                Country = new TestableCountry() { Id = countryId },
                CompetentAuthority = new TestableCompetentAuthority()
            };
            var transportRoute = new TestableTransportRoute()
            {
                TransitStates = new List<TransitState>() { transitState }
            };

            A.CallTo(() => transportRouteRepository.GetByNotificationId(notificationId))
                .Returns(transportRoute);

            var result = await handler.HandleAsync(GetRequest(entryPointId, exitPointId));
            
            Assert.Equal(transitStateId, result);
        }

        private UpdateTransitStateEntryOrExit GetRequest(Guid? entryPoint, Guid? exitPoint)
        {
            return new UpdateTransitStateEntryOrExit(notificationId, transitStateId, entryPoint, exitPoint);
        }
    }
}
