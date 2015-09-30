namespace EA.Iws.DataAccess.Tests.Integration.Notification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Recovery;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;
    using NotificationType = Domain.NotificationApplication.NotificationType;

    [Trait("Category", "Integration")]
    public class RecoveryInfoIntegration
    {
        private readonly IwsContext context;
        private readonly IEventDispatcher eventDispatcher;

        public RecoveryInfoIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            eventDispatcher = A.Fake<IEventDispatcher>();

            context = new IwsContext(userContext, eventDispatcher);
        }

        [Fact]
        public async Task CanAddRecoveryInfo()
        {
            NotificationApplication notification = await CreateNotification();
            RecoveryInfo recoveryInfo = await CreateRecoveryInfo(notification);

            Assert.NotNull(context.RecoveryInfos.SingleOrDefault(ri => ri.NotificationId == notification.Id));

            await DeleteEntity(recoveryInfo);
            await DeleteEntity(notification);
        }

        [Fact]
        public async Task CanAddRecoveryInfo_WithoutDisposal()
        {
            NotificationApplication notification = await CreateNotification();
            RecoveryInfo recoveryInfo = await CreateRecoveryInfo(notification, withDisposal: false);

            Assert.NotNull(context.RecoveryInfos.SingleOrDefault(ri => ri.NotificationId == notification.Id));

            await DeleteEntity(recoveryInfo);
            await DeleteEntity(notification);
        }
        
        [Fact]
        public async Task CanAddRecoveryPercentageDataProvidedByImporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
            UKCompetentAuthority.England, 0);

            notification.SetRecoveryInformationProvider(ProvidedBy.Importer);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.RecoveryInformationProvidedByImporter);

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

        private async Task<RecoveryInfo> CreateRecoveryInfo(NotificationApplication notification, bool withDisposal = true)
        {
            var estimatedValue = new EstimatedValue(ValuePerWeightUnits.Kilogram, 10);
            var recoveryCost = new RecoveryCost(ValuePerWeightUnits.Tonne, 50);
            var disposalCost = new DisposalCost(ValuePerWeightUnits.Tonne, 55);
            
            RecoveryInfo recoveryInfo;
            if (withDisposal)
            {
                recoveryInfo = new RecoveryInfo(notification.Id, new Percentage(50), estimatedValue, recoveryCost, disposalCost);
            }
            else
            {
                recoveryInfo = new RecoveryInfo(notification.Id, new Percentage(100), estimatedValue, recoveryCost, new DisposalCost(null, null));
            }

            context.RecoveryInfos.Add(recoveryInfo);
            await context.SaveChangesAsync();
            return recoveryInfo;
        }

        private async Task DeleteEntity(Entity entity)
        {
            context.DeleteOnCommit(entity);
            await context.SaveChangesAsync();
        }
    }
}
