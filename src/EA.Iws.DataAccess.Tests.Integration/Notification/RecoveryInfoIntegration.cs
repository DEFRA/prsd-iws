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
    public class RecoveryInfoIntegration : IDisposable
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

        //[Fact]
        //public async Task RecoveryInfoExists_SetProviderToImporter_RemovesExistingInfo()
        //{
        //    NotificationApplication notification = await CreateNotification();
        //    RecoveryInfo recoveryInfo = await CreateRecoveryInfo(notification);

        //    A.CallTo(() => eventDispatcher.Dispatch(A<ProviderChangedEvent>.Ignored)).Invokes(new ProviderChangedEventHandler(context, ));

        //    notification.SetRecoveryInformationProvider(ProvidedBy.Importer);
        //    await context.SaveChangesAsync();

        //    Assert.Null(context.RecoveryInfos.SingleOrDefault(ri => ri.NotificationId == notification.Id));

        //    await DeleteEntity(recoveryInfo);
        //    await DeleteEntity(notification);
        //}

        [Fact]
        public async Task CanAddRecoveryPercentageData()
        {
            var recoveryPercentage = 56.78M;
            var methodOfDisposal = "Some method of disposal text";
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
            UKCompetentAuthority.England, 0);

            notification.SetPercentageRecoverable(recoveryPercentage);
            notification.SetMethodOfDisposal(methodOfDisposal);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.Equal(recoveryPercentage, notification.PercentageRecoverable);
            Assert.Equal(methodOfDisposal, notification.MethodOfDisposal);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
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
                recoveryInfo = new RecoveryInfo(notification.Id, estimatedValue, recoveryCost, disposalCost);
            }
            else
            {
                recoveryInfo = new RecoveryInfo(notification.Id, estimatedValue, recoveryCost, new DisposalCost(null, null));
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
