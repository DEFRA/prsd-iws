namespace EA.Iws.DataAccess.Tests.Integration.Notification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.WasteRecovery;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    [Trait("Category", "Integration")]
    public class WasteRecoveryIntegration
    {
        private readonly IwsContext context;
        private readonly IEventDispatcher eventDispatcher;

        public WasteRecoveryIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            eventDispatcher = A.Fake<IEventDispatcher>();

            context = new IwsContext(userContext, eventDispatcher);
        }

        //TODO: write new integration tests that test the creation, updation and deletion of waste recovery and disposal information.

        private async Task<NotificationApplication> CreateNotification()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            return notification;
        }

        private async Task<WasteRecovery> CreateWasteRecovery(NotificationApplication notification)
        {
            var estimatedValue = new EstimatedValue(ValuePerWeightUnits.Kilogram, 10);
            var recoveryCost = new RecoveryCost(ValuePerWeightUnits.Tonne, 50);

            var wasteRecovery = new WasteRecovery(notification.Id, new Percentage(50), estimatedValue, recoveryCost);

            context.WasteRecoveries.Add(wasteRecovery);
            await context.SaveChangesAsync();
            return wasteRecovery;
        }

        private async Task DeleteEntity(Entity entity)
        {
            context.DeleteOnCommit(entity);
            await context.SaveChangesAsync();
        }
    }
}
