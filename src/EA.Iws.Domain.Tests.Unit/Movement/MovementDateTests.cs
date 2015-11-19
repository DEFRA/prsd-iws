namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using FakeItEasy;
    using TestHelpers.Helpers;
    using Xunit;

    public class MovementDateTests
    {
        private static readonly Guid AnyGuid = new Guid("5FE8E146-2584-43CF-A2A7-FD3911924502");
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private readonly GetOriginalDate originalDateService;
        private readonly IMovementDateHistoryRepository historyRepository;

        public MovementDateTests()
        {
            historyRepository = A.Fake<IMovementDateHistoryRepository>();
            originalDateService = new GetOriginalDate(historyRepository);
        }

        [Theory]
        [InlineData(MovementStatus.New)]
        [InlineData(MovementStatus.Cancelled)]
        [InlineData(MovementStatus.Completed)]
        [InlineData(MovementStatus.Received)]
        [InlineData(MovementStatus.Rejected)]
        public async Task MovementStatusNotSubmitted_Throws(MovementStatus status)
        {
            var movement = new Movement(1, AnyGuid, AnyDate);

            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movement);

            await Assert.ThrowsAsync<InvalidOperationException>(() => movement.UpdateDate(AnyDate.AddDays(1), originalDateService));
        }

        [Fact]
        public async Task DateUpdates()
        {
            var movement = new Movement(1, AnyGuid, AnyDate);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var newDate = AnyDate.AddDays(5);

            await movement.UpdateDate(newDate, originalDateService);

            Assert.Equal(newDate, movement.Date);
        }

        [Fact]
        public async Task DateMoreThan10DaysLater_Throws()
        {
            var movement = new Movement(1, AnyGuid, AnyDate);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var newDate = AnyDate.AddDays(11);

            await Assert.ThrowsAsync<InvalidOperationException>(() => movement.UpdateDate(newDate, originalDateService));
        }

        [Fact]
        public async Task DateChangedRaisesEvent()
        {
            var movement = new Movement(1, AnyGuid, AnyDate);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var newDate = AnyDate.AddDays(5);

            await movement.UpdateDate(newDate, originalDateService);

            Assert.NotNull(movement.Events.OfType<MovementDateChangeEvent>().SingleOrDefault());
        }

        [Fact]
        public async Task DateChangedEvent_ContainsOldDate()
        {
            var initialDate = AnyDate;
            var movement = new Movement(1, AnyGuid, initialDate);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var newDate = initialDate.AddDays(5);

            await movement.UpdateDate(newDate, originalDateService);

            Assert.Equal(initialDate, movement.Events.OfType<MovementDateChangeEvent>().Single().PreviousDate);
        }
    }
}