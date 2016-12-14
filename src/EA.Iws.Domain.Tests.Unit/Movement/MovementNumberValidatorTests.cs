namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using FakeItEasy;
    using Xunit;

    public class MovementNumberValidatorTests
    {
        private static readonly Guid NotificationId = new Guid("C30BAC4B-F01F-4D20-98CB-AF74A651BA15");
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");

        private readonly IMovementNumberValidator validator;
        private readonly IMovementRepository repository;

        public MovementNumberValidatorTests()
        {
            repository = A.Fake<IMovementRepository>();
            validator = new MovementNumberValidator(repository);
        }

        [Fact]
        public async Task NotificationNoMovements_AnyMovementNumber_ReturnsTrue()
        {
            A.CallTo(() => repository.GetAllMovements(NotificationId)).Returns(new Movement[0]);

            var result = await validator.Validate(NotificationId, 1);

            Assert.True(result);
        }

        [Fact]
        public async Task NotificationHasMovements_NumberExists_ReturnsFalse()
        {
            A.CallTo(() => repository.GetAllMovements(NotificationId)).Returns(new[]
            {
                new Movement(1, NotificationId, AnyDate, userId)
            });

            var result = await validator.Validate(NotificationId, 1);

            Assert.False(result);
        }

        [Fact]
        public async Task NotificationHasMovements_NoMatchingNumber_ReturnsTrue()
        {
            A.CallTo(() => repository.GetAllMovements(NotificationId)).Returns(new[]
            {
                new Movement(2, NotificationId, AnyDate, userId)
            });

            var result = await validator.Validate(NotificationId, 1);

            Assert.True(result);
        }
    }
}
