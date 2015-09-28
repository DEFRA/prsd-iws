namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using Core.Shared;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class RecoveryInfoViewModelTests
    {
        private readonly WasteRecoveryFormatter wasteRecoveryFormatter = new WasteRecoveryFormatter();

        [Fact]
        public void CanConstructWithNullNotification()
        {
            var model = new WasteRecoveryViewModel(null, null, wasteRecoveryFormatter);

            AssertAllModelStringAreEmpty(model);
        }

        [Fact]
        public void CanConstructWithNullWasteRecovery()
        {
            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(), null, wasteRecoveryFormatter);

            AssertAllModelStringAreEmpty(model);
        }

        [Fact(Skip = "Recovery percentage refactored, not yet reimplemented")]
        public void CanConstructWithPercentageRecoverableSet()
        {
            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(), null, wasteRecoveryFormatter);

            AssertAnnexMessageEmpty(model);
            AssertAllWasteRecoveryStringsAreEmpty(model);
            Assert.Equal("100%", model.PercentageRecoverable);
            Assert.Equal(string.Empty, model.MethodOfDisposal);
        }

        [Fact(Skip = "Recovery percentage refactored, not yet reimplemented")]
        public void CanConstructWithPercentageRecoverableAndMethodOfDisposalSet()
        {
            var methodOfDisposal = "throw in the bin";

            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(), null, wasteRecoveryFormatter);

            AssertAllWasteRecoveryStringsAreEmpty(model);
            AssertAnnexMessageEmpty(model);
            Assert.Equal("50%", model.PercentageRecoverable);
            Assert.Equal(methodOfDisposal, model.MethodOfDisposal);
        }

        [Fact(Skip = "Recovery percentage refactored, not yet reimplemented")]
        public void CanConstructWithWasteRecoveryInformationWhereDisposalInfoIsNull()
        {
            var methodOfDisposal = "smash it to bits";

            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(),
                new TestableWasteRecovery
                {
                    EstimatedValue = new EstimatedValue(ValuePerWeightUnits.Tonne, 250),
                    RecoveryCost = new RecoveryCost(ValuePerWeightUnits.Kilogram, 100),
                },
                wasteRecoveryFormatter);

            AssertAnnexMessageEmpty(model);
            Assert.Equal("50%", model.PercentageRecoverable);
            Assert.Equal(methodOfDisposal, model.MethodOfDisposal);
            Assert.Equal("£100 per Kilogram", model.CostAmountText);
            Assert.Equal("£250 per Tonne", model.EstimatedAmountText);
            Assert.Equal(string.Empty, model.DisposalAmountText);
        }

        [Fact(Skip = "Recovery percentage refactored, not yet reimplemented")]
        public void CanConstructWithWasteRecoveryInformationWithDisposalInfo()
        {
            var methodOfDisposal = "recycle";

            var model = new WasteRecoveryViewModel(new TestableNotificationApplication(),
                new TestableWasteRecovery
                {
                    EstimatedValue = new EstimatedValue(ValuePerWeightUnits.Tonne, 110),
                    RecoveryCost = new RecoveryCost(ValuePerWeightUnits.Kilogram, 100),
                    //DisposalCost = new DisposalCost(ValuePerWeightUnits.Kilogram, 90)
                },
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
