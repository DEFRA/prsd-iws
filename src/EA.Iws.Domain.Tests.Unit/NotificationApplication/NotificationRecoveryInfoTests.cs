namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Linq;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Recovery;
    using Xunit;
    using NotificationType = Domain.NotificationApplication.NotificationType;

    public class NotificationRecoveryInfoTests
    {
        [Fact]
        public void CanAddRecoveryInfoValues()
        {
            var estimatedValue = new EstimatedValue(ValuePerWeightUnits.Kilogram, 10);
            var recoveryCost = new RecoveryCost(ValuePerWeightUnits.Tonne, 50);
            var disposalCost = new DisposalCost(ValuePerWeightUnits.Tonne, 55);

            var recoveryInfo = new RecoveryInfo(Guid.NewGuid(), estimatedValue, recoveryCost, disposalCost);

            Assert.NotNull(recoveryInfo);
        }

        [Fact]
        public void CanAddRecoveryInfoValues_WithoutDisposal()
        {
            var estimatedValue = new EstimatedValue(ValuePerWeightUnits.Kilogram, 10);
            var recoveryCost = new RecoveryCost(ValuePerWeightUnits.Tonne, 50);

            var recoveryInfo = new RecoveryInfo(Guid.NewGuid(), estimatedValue, recoveryCost, null);

            Assert.NotNull(recoveryInfo);
        }

        [Fact]
        public void SetProvider_RaisesEvent()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.SetRecoveryInformationProvider(ProvidedBy.Importer);

            Assert.Equal(ProvidedBy.Importer, notification.Events.OfType<ProviderChangedEvent>().SingleOrDefault().NewProvider);
        }
    }
}
