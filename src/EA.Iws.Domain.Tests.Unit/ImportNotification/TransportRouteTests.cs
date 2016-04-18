namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using System.Linq;
    using Domain.ImportNotification;
    using Xunit;

    public class TransportRouteTests
    {
        private readonly Guid anyGuid = new Guid("DE6CA75B-41B3-4781-ABE9-6BF09C9FC639");
        private readonly StateOfImport stateOfImport;
        private readonly StateOfExport stateOfExport;

        public TransportRouteTests()
        {
            stateOfImport = new StateOfImport(anyGuid, anyGuid);
            stateOfExport = new StateOfExport(anyGuid, anyGuid, anyGuid);
        }

        [Fact]
        public void CanCreateTransportRoute()
        {
            var transportRoute = new TransportRoute(anyGuid, stateOfExport, stateOfImport);

            Assert.IsType<TransportRoute>(transportRoute);
        }

        [Fact]
        public void CanSetTransitStates()
        {
            var transportRoute = new TransportRoute(anyGuid, stateOfExport, stateOfImport);

            var transitStates = new[]
            {
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 1),
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 2),
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 3)
            };

            var transitStatesList = new TransitStateList(transitStates);

            transportRoute.SetTransitStates(transitStatesList);

            Assert.Equal(3, transportRoute.TransitStates.Count());
        }
    }
}