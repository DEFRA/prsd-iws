namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Linq;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.WasteRecovery;
    using Xunit;
    using NotificationType = Domain.NotificationApplication.NotificationType;

    public class NotificationWasteRecoveryTests
    {
        [Fact]
        public void CanAddWasteRecoveryValues()
        {
            var estimatedValue = new EstimatedValue(ValuePerWeightUnits.Kilogram, 10);
            var recoveryCost = new RecoveryCost(ValuePerWeightUnits.Tonne, 50);

            var wasteRecovery = new WasteRecovery(Guid.NewGuid(), new Percentage(50), estimatedValue, recoveryCost);

            Assert.NotNull(wasteRecovery);
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

            notification.SetWasteRecoveryInformationProvider(ProvidedBy.Importer);

            Assert.Equal(ProvidedBy.Importer, notification.Events.OfType<ProviderChangedEvent>().SingleOrDefault().NewProvider);
        }

        [Fact]
        public void UpdateWasteRecovery_RaisesEvent()
        {
            var wasteRecovery = new WasteRecovery(
                Guid.NewGuid(),
                new Percentage(50m),
                new EstimatedValue(ValuePerWeightUnits.Kilogram, 50m),
                new RecoveryCost(ValuePerWeightUnits.Kilogram, 40m));

            wasteRecovery.Update(new Percentage(60m), new EstimatedValue(ValuePerWeightUnits.Tonne, 50000m), new RecoveryCost(ValuePerWeightUnits.Tonne, 40000m));

            Assert.Equal(60m, wasteRecovery.Events.OfType<PercentageChangedEvent>().SingleOrDefault().NewPercentage.Value);
        }
    }
}
