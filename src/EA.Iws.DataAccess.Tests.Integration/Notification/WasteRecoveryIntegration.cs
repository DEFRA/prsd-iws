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
    using NotificationType = Domain.NotificationApplication.NotificationType;

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

        [Fact]
        public async Task CanAddWasteRecovery()
        {
            NotificationApplication notification = await CreateNotification();
            WasteRecovery wasteRecovery = await CreateWasteRecovery(notification);

            Assert.NotNull(context.WasteRecoveries.SingleOrDefault(ri => ri.NotificationId == notification.Id));

            await DeleteEntity(wasteRecovery);
            await DeleteEntity(notification);
        }

        [Fact]
        public async Task CanAddWasteRecovery_WithoutDisposal()
        {
            NotificationApplication notification = await CreateNotification();
            WasteRecovery wasteRecovery = await CreateWasteRecovery(notification, withDisposal: false);

            Assert.NotNull(context.WasteRecoveries.SingleOrDefault(ri => ri.NotificationId == notification.Id));

            await DeleteEntity(wasteRecovery);
            await DeleteEntity(notification);
        }
        
        [Fact]
        public async Task CanAddRecoveryPercentageDataProvidedByImporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
            UKCompetentAuthority.England, 0);

            notification.SetWasteRecoveryInformationProvider(ProvidedBy.Importer);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.WasteRecoveryInformationProvidedByImporter);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        private async Task<NotificationApplication> CreateNotification()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            return notification;
        }

        private async Task<WasteRecovery> CreateWasteRecovery(NotificationApplication notification, bool withDisposal = true)
        {
            var estimatedValue = new EstimatedValue(ValuePerWeightUnits.Kilogram, 10);
            var recoveryCost = new RecoveryCost(ValuePerWeightUnits.Tonne, 50);
            var disposalCost = new DisposalCost(ValuePerWeightUnits.Tonne, 55);
            
            WasteRecovery wasteRecovery;
            if (withDisposal)
            {
                wasteRecovery = new WasteRecovery(notification.Id, new Percentage(50), estimatedValue, recoveryCost, disposalCost);
            }
            else
            {
                wasteRecovery = new WasteRecovery(notification.Id, new Percentage(100), estimatedValue, recoveryCost, new DisposalCost(null, null));
            }

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
