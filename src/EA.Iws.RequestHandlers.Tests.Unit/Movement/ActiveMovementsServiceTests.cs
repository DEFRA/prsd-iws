namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using EA.Iws.RequestHandlers.Movement;
    using EA.Iws.TestHelpers.DomainFakes;
    using EA.Prsd.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ActiveMovementsServiceTests : TestBase
    {
        private readonly ActiveMovementsService service;

        public ActiveMovementsServiceTests()
        {
            Context.Movements.Add(Movement);
            Context.NotificationApplications.Add(NotificationApplication);

            service = new ActiveMovementsService(Context);
        }

        [Fact]
        public async Task ReturnsTotalActiveMovements()
        {
            SystemTime.Freeze(new DateTime(2015, 1, 1));

            Context.Movements.AddRange(new[]
            {
                new TestableMovement
                {
                    NotificationApplicationId = NotificationId,
                    Date = new DateTime(2014, 9, 7)
                },
                new TestableMovement
                {
                    NotificationApplicationId = NotificationId,
                    Date = new DateTime(2014, 11, 15)
                },
                new TestableMovement
                {
                    NotificationApplicationId = NotificationId,
                    Date = new DateTime(2015, 2, 4)
                }
            });

            Assert.Equal(2, await service.TotalActiveMovementsAsync(NotificationId));

            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task ReturnsZeroWhenNoActiveMovements()
        {
            Assert.Equal(0, await service.TotalActiveMovementsAsync(NotificationId));
        }
    }
}
