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

    public class SetPackagingDataForMovementHandlerTests
    {
        private static readonly Guid UserId = new Guid("5C21D521-D62D-40A2-84B1-BB3F20EA7531");
        private static readonly Guid NotificationId = new Guid("DB016271-09C0-4317-B1A7-F10A42667F15");
        private static readonly Guid MovementId = new Guid("B495A9BB-9D70-4583-828E-307DB9B4B725");

        private readonly IwsContext context;
        private readonly SetPackagingDataForMovementHandler handler;
        private readonly TestableNotificationApplication notificationApplication;
        private readonly TestableMovement movement;

        public SetPackagingDataForMovementHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(UserId));

            var packagingInfos = new TestablePackagingInfo[]
            {
                new TestablePackagingInfo{ PackagingType = PackagingType.Drum },
                new TestablePackagingInfo{ PackagingType = PackagingType.Jerrican }
            };

            notificationApplication = new TestableNotificationApplication
            {
                Id = NotificationId,
                PackagingInfos = packagingInfos
            };

            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationApplication = notificationApplication,
                PackagingInfos = new List<TestablePackagingInfo>()
            };

            context.Movements.Add(movement);

            handler = new SetPackagingDataForMovementHandler(context);
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

            var movement = context.Movements.Single(m => m.Id == MovementId);

            var packagingTypesResult = movement.PackagingInfos.Select(info => info.PackagingType);

            Assert.Equal(packagingTypes.Length, packagingTypesResult.Count());
        }
    }
}
