namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;
    using PackagingTypeEnum = Core.PackagingType.PackagingType;

    public class GetPackagingDataValidForMovementHandlerTests
    {
        private static readonly Guid UserId = new Guid("19FF07B5-2D32-411D-B446-9D334984F6E8");
        private static readonly Guid NotificationId = new Guid("BBC91F82-22B7-49C4-BEEB-E2EF59F11795");
        private static readonly Guid MovementId = new Guid("8E80E109-2664-4397-A21E-4C0132858241");

        private readonly IwsContext context;
        private readonly GetPackagingDataValidForMovementHandler handler;
        private readonly TestableNotificationApplication notificationApplication;
        private readonly TestableMovement movement;
        private readonly GetPackagingDataValidForMovement request;

        public GetPackagingDataValidForMovementHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(UserId));

            var packagingInfos = new TestablePackagingInfo[]
            {
                new TestablePackagingInfo{ PackagingType = PackagingType.Bag },
                new TestablePackagingInfo{ PackagingType = PackagingType.CompositePackaging },
                new TestablePackagingInfo{ PackagingType = PackagingType.Other, OtherDescription = "Carrier bag" }
            };
            
            notificationApplication = new TestableNotificationApplication
            {
                Id = NotificationId,
                UserId = UserId,
                PackagingInfos = packagingInfos
            };

            context.NotificationApplications.Add(notificationApplication);

            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationId = NotificationId
            };

            context.Movements.Add(movement);

            handler = new GetPackagingDataValidForMovementHandler(context);
            request = new GetPackagingDataValidForMovement(MovementId);
        }

        [Fact]
        public async Task MovementDoesNotExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new GetPackagingDataValidForMovement(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsPackagingTypes()
        {
            var result = await handler.HandleAsync(request);

            var expected = new PackagingTypeEnum[]
            {
                PackagingTypeEnum.Bag,
                PackagingTypeEnum.CompositePackaging,
                PackagingTypeEnum.Other
            };

            Assert.Equal(expected.OrderBy(pt => pt).ToList(), result.PackagingTypes.OrderBy(pt => pt).ToList());
        }
    }
}
