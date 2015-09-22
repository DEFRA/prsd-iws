namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;
    using PackagingTypeEnum = Core.PackagingType.PackagingType;

    public class SetPackagingDataForMovementHandlerTests : TestBase
    {
        private readonly SetPackagingDataForMovementHandler handler;

        public SetPackagingDataForMovementHandlerTests()
        {
            var packagingInfos = new TestablePackagingInfo[]
            {
                new TestablePackagingInfo{ PackagingType = PackagingType.Drum },
                new TestablePackagingInfo{ PackagingType = PackagingType.Jerrican }
            };

            Movement.PackagingInfos = new List<PackagingInfo>();

            NotificationApplication.PackagingInfos = packagingInfos;

            Context.Movements.Add(Movement);
            Context.NotificationApplications.Add(NotificationApplication);

            handler = new SetPackagingDataForMovementHandler(Context);
        }

        [Fact]
        public async Task MovementNotExists_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new SetPackagingDataForMovement(Guid.Empty, null)));
        }

        [Fact]
        public async Task CreatesLinkBetweenMovementAndPackagingInfos()
        {
            var packagingTypes = new PackagingTypeEnum[]
            {
                PackagingTypeEnum.Drum,
                PackagingTypeEnum.Jerrican
            };

            var result = await handler.HandleAsync(new SetPackagingDataForMovement(MovementId, packagingTypes.ToList()));

            var movement = Context.Movements.Single(m => m.Id == MovementId);

            var packagingTypesResult = movement.PackagingInfos.Select(info => info.PackagingType);

            Assert.Equal(packagingTypes.Length, packagingTypesResult.Count());
        }
    }
}
