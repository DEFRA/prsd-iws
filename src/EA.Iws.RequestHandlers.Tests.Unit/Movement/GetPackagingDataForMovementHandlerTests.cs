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

    public class GetPackagingDataForMovementHandlerTests
    {
        private static readonly Guid UserId = new Guid("201B4F05-4B05-423E-B72A-0A0A9B620D73");
        private static readonly Guid NotificationId = new Guid("BCA3D29F-90E6-4B50-B401-6C3D32EF92D6");
        private static readonly Guid MovementId = new Guid("0914C9D2-B952-4851-99DE-098D83907C5C");

        private readonly IwsContext context;
        private readonly GetPackagingDataForMovementHandler handler;
        private readonly TestableMovement movement;
        private readonly GetPackagingDataForMovement request;

        public GetPackagingDataForMovementHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(UserId));

            var packagingInfos = new TestablePackagingInfo[]
            {
                new TestablePackagingInfo{ PackagingType = PackagingType.Bag },
                new TestablePackagingInfo{ PackagingType = PackagingType.CompositePackaging },
                new TestablePackagingInfo{ PackagingType = PackagingType.Other, OtherDescription = "Carrier bag" }
            };
            
            movement = new TestableMovement
            {
                Id = MovementId,
                PackagingInfos = packagingInfos
            };

            context.Movements.Add(movement);

            handler = new GetPackagingDataForMovementHandler(context);
            request = new GetPackagingDataForMovement(MovementId);
        }

        [Fact]
        public async Task MovementDoesNotExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new GetPackagingDataForMovement(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsPackagingTypes()
        {
            var result = await handler.HandleAsync(request);

            var expected = new[]
            {
                PackagingTypeEnum.Bag,
                PackagingTypeEnum.CompositePackaging,
                PackagingTypeEnum.Other
            };

            Assert.Equal(
                expected.OrderBy(pt => pt).ToList(),
                result.PackagingTypes.OrderBy(pt => pt).ToList());
        }
    }
}
