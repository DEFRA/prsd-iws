namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetNumberOfPackagesByMovementIdHandlerTests
    {
        private static readonly Guid MovementId = new Guid("D419DAC2-43C1-48A5-A6DF-22E055EDAEA0");

        private readonly SetNumberOfPackagesByMovementIdHandler handler;
        private readonly TestableMovement movement;
        private readonly TestIwsContext context;

        public SetNumberOfPackagesByMovementIdHandlerTests()
        {
            context = new TestIwsContext();
            handler = new SetNumberOfPackagesByMovementIdHandler(context);
            
            movement = new TestableMovement
            {
                Id = MovementId,
                NumberOfPackages = null
            };

            context.Movements.Add(movement);
        }

        [Fact]
        public async Task SetToZero_Throws()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                handler.HandleAsync(new SetNumberOfPackagesByMovementId(MovementId, 0)));
        }

        [Fact]
        public async Task SetToNegativeNumber_Throws()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                handler.HandleAsync(new SetNumberOfPackagesByMovementId(MovementId, -5)));
        }

        [Fact]
        public async Task SetForNonExistentMovement_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new SetNumberOfPackagesByMovementId(Guid.Empty, 1)));
        }

        [Fact]
        public async Task SetToPositiveNumber_Sets()
        {
            await handler.HandleAsync(new SetNumberOfPackagesByMovementId(MovementId, 7));

            Assert.Equal(7, movement.NumberOfPackages);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(new SetNumberOfPackagesByMovementId(MovementId, 7));

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}
