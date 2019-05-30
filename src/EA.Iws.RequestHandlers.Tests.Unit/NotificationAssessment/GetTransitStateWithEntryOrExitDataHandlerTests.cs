namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.TransitState;
    using Core.TransportRoute;
    using Domain.TransportRoute;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using RequestHandlers.NotificationAssessment;
    using Requests.NotificationAssessment;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetTransitStateWithEntryOrExitDataHandlerTests
    {
        private readonly GetTransitStateWithEntryOrExitDataHandler handler;
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;

        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid transitStateId = Guid.NewGuid();
        private readonly Guid countryId = Guid.NewGuid();
        private const string AnyString = "test";
        private const int EntryExitPoints = 2;

        public GetTransitStateWithEntryOrExitDataHandlerTests()
        {
            mapper = A.Fake<IMapper>();
            transportRouteRepository = A.Fake<ITransportRouteRepository>();

            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));
            context.EntryOrExitPoints.AddRange(new List<EntryOrExitPoint>(EntryExitPoints)
            {
                new TestableEntryOrExitPoint()
                {
                    Country = new TestableCountry() { Id = countryId }
                },
                new TestableEntryOrExitPoint()
                {
                    Country = new TestableCountry() { Id = countryId }
                }
            });

            A.CallTo(() => mapper.Map<EntryOrExitPointData>(A<EntryOrExitPoint>.Ignored))
                .Returns(new EntryOrExitPointData() {CountryId = countryId, Name = AnyString});

            handler = new GetTransitStateWithEntryOrExitDataHandler(context, mapper, transportRouteRepository);
        }

        [Fact]
        public async Task GetTransitStateWithEntryOrExitDataHandler_NoTransportRoute_ReturnsNotNull()
        {
            A.CallTo(() => transportRouteRepository.GetByNotificationId(notificationId)).Returns((TransportRoute)null);

            var result = await handler.HandleAsync(GetRequest());

            Assert.NotNull(result);
            Assert.NotNull(result.TransitState);
            Assert.NotNull(result.EntryOrExitPoints);
        }

        [Fact]
        public async Task GetTransitStateWithEntryOrExitDataHandler_NoTransportRoute_ReturnsEmptyData()
        {
            A.CallTo(() => transportRouteRepository.GetByNotificationId(notificationId)).Returns((TransportRoute)null);

            var result = await handler.HandleAsync(GetRequest());

            Assert.NotNull(result);
            Assert.Null(result.TransitState.EntryPoint);
            Assert.Null(result.TransitState.ExitPoint);
            Assert.Empty(result.EntryOrExitPoints);
        }

        [Fact]
        public async Task GetTransitStateWithEntryOrExitDataHandler_HasTransportRoute_NoTransitState_ReturnsNotNull()
        {
            A.CallTo(() => transportRouteRepository.GetByNotificationId(notificationId))
                .Returns(new TestableTransportRoute() { Id = Guid.NewGuid() });

            var result = await handler.HandleAsync(GetRequest());

            Assert.NotNull(result);
            Assert.NotNull(result.TransitState);
            Assert.NotNull(result.EntryOrExitPoints);
        }

        [Fact]
        public async Task GetTransitStateWithEntryOrExitDataHandler_HasTransportRoute_NoTransitState_ReturnsEmptyData()
        {
            A.CallTo(() => transportRouteRepository.GetByNotificationId(notificationId))
                .Returns(new TestableTransportRoute() { Id = Guid.NewGuid() });

            var result = await handler.HandleAsync(GetRequest());

            Assert.NotNull(result);
            Assert.Null(result.TransitState.EntryPoint);
            Assert.Null(result.TransitState.ExitPoint);
            Assert.Empty(result.EntryOrExitPoints);
        }

        [Fact]
        public async Task GetTransitStateWithEntryOrExitDataHandler_HasTransitState_ReturnsData()
        {
            var transitState = new TestableTransitState()
            {
                Id = transitStateId,
                Country = new TestableCountry() { Id = countryId }
            };
            var transportRoute = new TestableTransportRoute()
            {
                TransitStates = new List<TransitState>() { transitState }
            };

            A.CallTo(() => transportRouteRepository.GetByNotificationId(notificationId))
                .Returns(transportRoute);
            A.CallTo(() => mapper.Map<TransitStateData>(transitState))
                .Returns(new TransitStateData() { Id = transitStateId });

            var result = await handler.HandleAsync(GetRequest());

            Assert.Equal(transitStateId, result.TransitState.Id);
            Assert.Equal(EntryExitPoints, result.EntryOrExitPoints.Count());
            Assert.All(result.EntryOrExitPoints, ep => Assert.Equal(countryId, ep.CountryId));
        }

        private GetTransitStateWithEntryOrExitData GetRequest()
        {
            return new GetTransitStateWithEntryOrExitData(notificationId, transitStateId);
        }
    }
}