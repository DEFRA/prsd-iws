namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using FakeItEasy;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetOriginalDateTests
    {
        private static readonly Guid AnyGuid = new Guid("0FBE9BBE-76DA-4028-AF36-D516E207F345");
        private static readonly DateTime AnyDate = new DateTime(2014, 12, 25);
        private readonly IMovementDateHistoryRepository historyRepository;

        public GetOriginalDateTests()
        {
            historyRepository = A.Fake<IMovementDateHistoryRepository>();
        }

        [Fact]
        public async Task WhenNoPreviousDates_ReturnsMovementDate()
        {
            var movementDate = new DateTime(2015, 1, 1);
            var movement = new Movement(1, AnyGuid, movementDate);
            var dateService = new OriginalMovementDate(historyRepository);

            Assert.Equal(movementDate, await dateService.Get(movement));
        }

        [Fact]
        public async Task WhenOnePreviousDate_ReturnsPreviousDate()
        {
            var movement = new Movement(1, AnyGuid, AnyDate);

            var previousDate = new DateTime(2015, 1, 2);

            A.CallTo(() => historyRepository.GetByMovementId(A<Guid>.Ignored))
                .Returns(new[]
                {
                    new MovementDateHistory(AnyGuid, previousDate)
                });

            var dateService = new OriginalMovementDate(historyRepository);

            Assert.Equal(previousDate, await dateService.Get(movement));
        }

        [Fact]
        public async Task WhenMultiPreviousDates_ReturnsOldestPreviousDate()
        {
            var movement = new Movement(1, AnyGuid, AnyDate);

            var oldestDateHistory = new MovementDateHistory(AnyGuid, new DateTime(2015, 1, 5));
            ObjectInstantiator<MovementDateHistory>.SetProperty(m => m.DateChanged, new DateTime(2015, 10, 10), oldestDateHistory);

            var otherDateHistory = new MovementDateHistory(AnyGuid, new DateTime(2015, 1, 3));
            ObjectInstantiator<MovementDateHistory>.SetProperty(m => m.DateChanged, new DateTime(2015, 10, 11), otherDateHistory);

            A.CallTo(() => historyRepository.GetByMovementId(A<Guid>.Ignored))
                .Returns(new[]
                {
                    oldestDateHistory,
                    otherDateHistory
                });

            var dateService = new OriginalMovementDate(historyRepository);

            Assert.Equal(oldestDateHistory.PreviousDate, await dateService.Get(movement));
        }
    }
}