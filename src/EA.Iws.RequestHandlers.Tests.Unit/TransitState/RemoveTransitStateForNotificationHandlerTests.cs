namespace EA.Iws.RequestHandlers.Tests.Unit.TransitState
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using FakeItEasy;
    using RequestHandlers.TransitState;
    using Requests.TransitState;
    using TestHelpers.Helpers;
    using Xunit;

    public class RemoveTransitStateForNotificationHandlerTests
    {
        private static readonly Guid ExistingNotificationId = new Guid("9EEA43DD-3381-45DE-9027-02F059995344");
        private static readonly Guid ExistingTransitStateId = new Guid("298F72E0-E57E-43D7-9200-5238633D72D5");
        private static readonly Guid NonExistingNotificationId = new Guid("0C5995F8-D02B-4391-8AD5-954DCEDCB290");
        private readonly RemoveTransitStateForNotificationHandler handler;
        private readonly IwsContext context;

        public RemoveTransitStateForNotificationHandlerTests()
        {
            context = A.Fake<IwsContext>();
            var helper = new DbContextHelper();

            var notification = NotificationApplicationFactory.Create(ExistingNotificationId);

            notification.AddTransitStateToNotification(TransitStateFactory.Create(ExistingTransitStateId, 
                CountryFactory.Create(new Guid("3E7A0092-B6CB-46AD-ABCC-FB741EB6CF35")), 1));

            A.CallTo(() => context.NotificationApplications).Returns(helper.GetAsyncEnabledDbSet(new[]
            {
                notification
            }));

            handler = new RemoveTransitStateForNotificationHandler(context);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        handler.HandleAsync(new RemoveTransitStateForNotification(NonExistingNotificationId, Guid.Empty)));
        }

        [Fact]
        public async Task TransitStateDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new RemoveTransitStateForNotification(ExistingNotificationId, Guid.Empty)));
        }

        [Fact]
        public async Task TransitStateExists_Removes()
        {
            await
                handler.HandleAsync(new RemoveTransitStateForNotification(ExistingNotificationId, ExistingTransitStateId));

            Assert.Empty(context.NotificationApplications.Single(na => na.Id == ExistingNotificationId).TransitStates);
        }
    }
}
