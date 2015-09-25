namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Xunit;

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
    }
}
