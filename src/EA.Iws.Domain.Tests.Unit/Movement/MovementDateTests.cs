namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;

    public class MovementDateTests : IDisposable
    {
        private static readonly Guid NotificationId = new Guid("5FE8E146-2584-43CF-A2A7-FD3911924502");
        private static readonly DateTime Today = new DateTime(2015, 1, 1);
        private readonly IUpdatedMovementDateValidator validator;

        public MovementDateTests()
        {
            validator = A.Fake<IUpdatedMovementDateValidator>();

            SystemTime.Freeze(Today);
        }

        [Fact]
        public async Task DateUpdates()
        {
            var movement = new Movement(1, NotificationId, Today);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var newDate = Today.AddDays(13);

            await movement.UpdateDate(newDate, validator);

            Assert.Equal(newDate, movement.Date);
        }

        [Fact]
        public async Task DateNotValid_Throws()
        {
            var initialDate = Today;
            var movement = new Movement(1, NotificationId, initialDate);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            A.CallTo(() => validator.EnsureDateValid(movement, A<DateTime>.Ignored)).Throws<MovementDateException>();

            var newDate = initialDate.AddDays(5);

            await Assert.ThrowsAsync<MovementDateException>(() => movement.UpdateDate(newDate, validator));
        }

        [Fact]
        public async Task DateChangedRaisesEvent()
        {
            var movement = new Movement(1, NotificationId, Today);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var newDate = Today.AddDays(5);

            await movement.UpdateDate(newDate, validator);

            Assert.NotNull(movement.Events.OfType<MovementDateChangeEvent>().SingleOrDefault());
        }

        [Fact]
        public async Task DateChangedEvent_ContainsOldDate()
        {
            var initialDate = Today;
            var movement = new Movement(1, NotificationId, initialDate);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var newDate = initialDate.AddDays(5);

            await movement.UpdateDate(newDate, validator);

            Assert.Equal(initialDate, movement.Events.OfType<MovementDateChangeEvent>().Single().PreviousDate);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}