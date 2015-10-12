namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System;
    using Core.Shared;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class RecoveryInfoViewModelTests
    {
        private readonly WasteRecoveryFormatter wasteRecoveryFormatter = new WasteRecoveryFormatter();

        [Fact]
        public void CanConstructWithNullNotification()
        {
            var model = new WasteRecoveryViewModel(null, null, null, wasteRecoveryFormatter);

            AssertAllModelStringAreEmpty(model);
        }

        [Fact]
        public void CanConstructWithNullWasteRecovery()
        {
            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(), null, null, wasteRecoveryFormatter);

            AssertAllModelStringAreEmpty(model);
        }

        [Fact]
        public void CanConstructWithPercentageRecoverableSet()
        {
            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(), 
                new WasteRecovery(Guid.NewGuid(), 
                    new Percentage(100), 
                    new EstimatedValue(ValuePerWeightUnits.Kilogram, 10), 
                    new RecoveryCost(ValuePerWeightUnits.Kilogram, 10)),
                null, wasteRecoveryFormatter);

            AssertAnnexMessageEmpty(model);
            Assert.Equal("100%", model.PercentageRecoverable);
            Assert.Equal(string.Empty, model.MethodOfDisposal);
        }

        [Fact]
        public void CanConstructWithPercentageRecoverableAndMethodOfDisposalSet()
        {
            var methodOfDisposal = "throw in the bin";
            var notificationId = Guid.NewGuid();

            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(), 
                new WasteRecovery(notificationId,
                    new Percentage(50),
                    new EstimatedValue(ValuePerWeightUnits.Kilogram, 10),
                    new RecoveryCost(ValuePerWeightUnits.Kilogram, 10)), 
                new WasteDisposal(notificationId, 
                    methodOfDisposal, 
                    new DisposalCost(ValuePerWeightUnits.Kilogram, 10)),
                wasteRecoveryFormatter);

            AssertAnnexMessageEmpty(model);
            Assert.Equal("50%", model.PercentageRecoverable);
            Assert.Equal(methodOfDisposal, model.MethodOfDisposal);
        }

        [Fact]
        public void CanConstructWithWasteRecoveryInformationWhereDisposalInfoIsNull()
        {
            var notificationId = Guid.NewGuid();

            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(),
                new WasteRecovery(notificationId,
                    new Percentage(100),
                    new EstimatedValue(ValuePerWeightUnits.Tonne, 250),
                    new RecoveryCost(ValuePerWeightUnits.Kilogram, 100)),
                null, 
                wasteRecoveryFormatter);

            AssertAnnexMessageEmpty(model);
            Assert.Equal("100%", model.PercentageRecoverable);
            Assert.Equal("£100 per Kilogram", model.CostAmountText);
            Assert.Equal("£250 per Tonne", model.EstimatedAmountText);
            Assert.Equal(string.Empty, model.DisposalAmountText);
        }

        [Fact]
        public void CanConstructWithWasteRecoveryInformationWithDisposalInfo()
        {
            var methodOfDisposal = "recycle";
            var notificationId = Guid.NewGuid();

            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(),
                new WasteRecovery(notificationId,
                    new Percentage(90),
                    new EstimatedValue(ValuePerWeightUnits.Tonne, 110),
                    new RecoveryCost(ValuePerWeightUnits.Kilogram, 100)),
                new WasteDisposal(notificationId,
                    methodOfDisposal,
                    new DisposalCost(ValuePerWeightUnits.Kilogram, 90)),
                wasteRecoveryFormatter);

            AssertAnnexMessageEmpty(model);
            Assert.Equal("90%", model.PercentageRecoverable);
            Assert.Equal(methodOfDisposal, model.MethodOfDisposal);
            Assert.Equal("£100 per Kilogram", model.CostAmountText);
            Assert.Equal("£90 per Kilogram", model.DisposalAmountText);
            Assert.Equal("£110 per Tonne", model.EstimatedAmountText);
        }

        private void AssertAllModelStringAreEmpty(WasteRecoveryViewModel model)
        {
            Assert.Equal(string.Empty, model.MethodOfDisposal);
            AssertAllWasteRecoveryStringsAreEmpty(model);
            AssertAnnexMessageEmpty(model);
            Assert.Equal(string.Empty, model.PercentageRecoverable);
        }

        private void AssertAllWasteRecoveryStringsAreEmpty(WasteRecoveryViewModel model)
        {
            Assert.Equal(string.Empty, model.CostAmountText);
            Assert.Equal(string.Empty, model.DisposalAmountText);
            Assert.Equal(string.Empty, model.EstimatedAmountText);
        }

        private void AssertAnnexMessageEmpty(WasteRecoveryViewModel model)
        {
            Assert.Equal(string.Empty, model.AnnexMessage);
        }
    }
}
