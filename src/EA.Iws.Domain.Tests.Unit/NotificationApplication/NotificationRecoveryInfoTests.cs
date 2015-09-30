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

            var recoveryInfo = new RecoveryInfo(Guid.NewGuid(), new Percentage(50), estimatedValue, recoveryCost, disposalCost);

            Assert.NotNull(recoveryInfo);
        }

        [Fact]
        public void CanAddRecoveryInfoValues_WithoutDisposal()
        {
            var estimatedValue = new EstimatedValue(ValuePerWeightUnits.Kilogram, 10);
            var recoveryCost = new RecoveryCost(ValuePerWeightUnits.Tonne, 50);

            var recoveryInfo = new RecoveryInfo(Guid.NewGuid(), new Percentage(100), estimatedValue, recoveryCost, null);

            Assert.NotNull(recoveryInfo);
        }

        [Fact]
        public void PercentageLessThanZero_Throws()
        {
            Action newPercentage = () => new Percentage(-1);

            Assert.Throws<ArgumentOutOfRangeException>(newPercentage);
        }

        [Fact]
        public void PercentageGreaterThan100_Throws()
        {
            Action newPercentage = () => new Percentage(110);

            Assert.Throws<ArgumentOutOfRangeException>(newPercentage);
        }

        [Theory]
        [InlineData(37.123, 37.12)]
        [InlineData(52.999, 53)]
        [InlineData(12.9876543, 12.99)]
        public void PercentageRoundsUpToTwoDecimalPlaces(double value, double expected)
        {
            var decimalValue = (decimal)value;
            var decimalExpected = (decimal)expected;

            var result = new Percentage(decimalValue);

            Assert.Equal(decimalExpected, result.Value);
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
