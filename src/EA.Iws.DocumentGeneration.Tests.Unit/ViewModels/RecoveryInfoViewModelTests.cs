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
        private readonly RecoveryInfoFormatter recoveryInfoFormatter = new RecoveryInfoFormatter();

        [Fact]
        public void CanConstructWithNullNotification()
        {
            var model = new RecoveryInfoViewModel(null, null, recoveryInfoFormatter);

            AssertAllModelStringAreEmpty(model);
        }

        [Fact]
        public void CanConstructWithNullRecoveryInfo()
        {
            var model = new RecoveryInfoViewModel(new TestableNotificationApplication(), null, recoveryInfoFormatter);

            AssertAllModelStringAreEmpty(model);
        }

        [Fact(Skip = "Recovery percentage refactored, not yet reimplemented")]
        public void CanConstructWithPercentageRecoverableSet()
        {
            var model = new RecoveryInfoViewModel(new TestableNotificationApplication(), null, recoveryInfoFormatter);

            AssertAnnexMessageEmpty(model);
            AssertAllRecoveryInfoStringsAreEmpty(model);
            Assert.Equal("100%", model.PercentageRecoverable);
            Assert.Equal(string.Empty, model.MethodOfDisposal);
        }

        [Fact(Skip = "Recovery percentage refactored, not yet reimplemented")]
        public void CanConstructWithPercentageRecoverableAndMethodOfDisposalSet()
        {
            var methodOfDisposal = "throw in the bin";

            var model = new RecoveryInfoViewModel(new TestableNotificationApplication(), null, recoveryInfoFormatter);

            AssertAllRecoveryInfoStringsAreEmpty(model);
            AssertAnnexMessageEmpty(model);
            Assert.Equal("50%", model.PercentageRecoverable);
            Assert.Equal(methodOfDisposal, model.MethodOfDisposal);
        }

        [Fact(Skip = "Recovery percentage refactored, not yet reimplemented")]
        public void CanConstructWithRecoveryInformationWhereDisposalInfoIsNull()
        {
            var methodOfDisposal = "smash it to bits";

            var model = new RecoveryInfoViewModel(new TestableNotificationApplication(),
                new TestableRecoveryInfo
                {
                    EstimatedValue = new EstimatedValue(ValuePerWeightUnits.Tonne, 250),
                    RecoveryCost = new RecoveryCost(ValuePerWeightUnits.Kilogram, 100),
                },
                recoveryInfoFormatter);

            AssertAnnexMessageEmpty(model);
            Assert.Equal("50%", model.PercentageRecoverable);
            Assert.Equal(methodOfDisposal, model.MethodOfDisposal);
            Assert.Equal("£100 per Kilogram", model.CostAmountText);
            Assert.Equal("£250 per Tonne", model.EstimatedAmountText);
            Assert.Equal(string.Empty, model.DisposalAmountText);
        }

        [Fact(Skip = "Recovery percentage refactored, not yet reimplemented")]
        public void CanConstructWithRecoveryInformationWithDisposalInfo()
        {
            var methodOfDisposal = "recycle";

            var model = new RecoveryInfoViewModel(new TestableNotificationApplication(),
                new TestableRecoveryInfo
                {
                    EstimatedValue = new EstimatedValue(ValuePerWeightUnits.Tonne, 110),
                    RecoveryCost = new RecoveryCost(ValuePerWeightUnits.Kilogram, 100),
                    DisposalCost = new DisposalCost(ValuePerWeightUnits.Kilogram, 90)
                },
                recoveryInfoFormatter);

            AssertAnnexMessageEmpty(model);
            Assert.Equal("90%", model.PercentageRecoverable);
            Assert.Equal(methodOfDisposal, model.MethodOfDisposal);
            Assert.Equal("£100 per Kilogram", model.CostAmountText);
            Assert.Equal("£90 per Kilogram", model.DisposalAmountText);
            Assert.Equal("£110 per Tonne", model.EstimatedAmountText);
        }

        private void AssertAllModelStringAreEmpty(RecoveryInfoViewModel model)
        {
            Assert.Equal(string.Empty, model.MethodOfDisposal);
            AssertAllRecoveryInfoStringsAreEmpty(model);
            AssertAnnexMessageEmpty(model);
            Assert.Equal(string.Empty, model.PercentageRecoverable);
        }

        private void AssertAllRecoveryInfoStringsAreEmpty(RecoveryInfoViewModel model)
        {
            Assert.Equal(string.Empty, model.CostAmountText);
            Assert.Equal(string.Empty, model.DisposalAmountText);
            Assert.Equal(string.Empty, model.EstimatedAmountText);
        }

        private void AssertAnnexMessageEmpty(RecoveryInfoViewModel model)
        {
            Assert.Equal(string.Empty, model.AnnexMessage);
        }
    }
}
