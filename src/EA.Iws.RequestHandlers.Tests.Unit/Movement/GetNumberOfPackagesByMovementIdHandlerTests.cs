namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetNumberOfPackagesByMovementIdHandlerTests
    {
        private static readonly Guid MovementId = new Guid("D419DAC2-43C1-48A5-A6DF-22E055EDAEA0");

        private readonly GetNumberOfPackagesByMovementIdHandler handler;
        private readonly TestableMovement movement;

        public GetNumberOfPackagesByMovementIdHandlerTests()
        {
            var context = new TestIwsContext();
            handler = new GetNumberOfPackagesByMovementIdHandler(context);
            
            movement = new TestableMovement
            {
                Id = MovementId,
                NumberOfPackages = null
            };

            context.Movements.Add(movement);
        }

        [Fact]
        public async Task MovementDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(() =>
                    handler.HandleAsync(new GetNumberOfPackagesByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task NumberOfPackagesIsNull_ReturnsNull()
        {
            var result = await handler.HandleAsync(new GetNumberOfPackagesByMovementId(MovementId));

            Assert.Null(result);
        }

        [Fact]
        public async Task NumberOfPackagesIsZero_ReturnsZero()
        {
            movement.NumberOfPackages = 0;

            var result = await handler.HandleAsync(new GetNumberOfPackagesByMovementId(MovementId));

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task NumberOfPackagesIsOne_ReturnsOne()
        {
            movement.NumberOfPackages = 1;

            var result = await handler.HandleAsync(new GetNumberOfPackagesByMovementId(MovementId));

            Assert.Equal(1, result);
        }
    }
}
