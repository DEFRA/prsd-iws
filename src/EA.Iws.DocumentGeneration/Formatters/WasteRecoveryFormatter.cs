namespace EA.Iws.DocumentGeneration.Formatters
{
    using System;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;

    internal class WasteRecoveryFormatter
    {
        private const string NotApplicable = "N/A";

        public string NullableDecimalAsPercentage(decimal? value)
        {
            if (value.HasValue)
            {
                return string.Format("{0}%", value.Value.ToString("G29"));
            }

            return string.Empty;
        }

        public string CostAmountWithUnits(WasteRecovery wasteRecovery, Func<WasteRecovery, ValuePerWeight> valuePerWeight)
        {
            if (wasteRecovery == null)
            {
                return NotApplicable;
            }

            return string.Format("£{0} per {1}", valuePerWeight(wasteRecovery).Amount, valuePerWeight(wasteRecovery).Units);
        }

        public string CostAmountWithUnits(WasteDisposal wasteDisposal, Func<WasteDisposal, DisposalCost> disposalCost)
        {
            if (wasteDisposal == null || disposalCost(wasteDisposal) == null)
            {
                return NotApplicable;
            }

            return string.Format("£{0} per {1}", disposalCost(wasteDisposal).Amount, disposalCost(wasteDisposal).Units);
        }
    }
}
