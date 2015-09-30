namespace EA.Iws.DocumentGeneration.Formatters
{
    using System;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;

    internal class WasteRecoveryFormatter
    {
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
                return string.Empty;
            }

            return string.Format("£{0} per {1}", valuePerWeight(wasteRecovery).Amount, valuePerWeight(wasteRecovery).Units);
        }

        public string CostAmountWithUnits(WasteRecovery wasteRecovery, Func<WasteRecovery, DisposalCost> disposalCost)
        {
            if (wasteRecovery == null || disposalCost(wasteRecovery) == null
                || !disposalCost(wasteRecovery).Amount.HasValue
                || !disposalCost(wasteRecovery).Units.HasValue)
            {
                return string.Empty;
            }

            return string.Format("£{0} per {1}", disposalCost(wasteRecovery).Amount, disposalCost(wasteRecovery).Units);
        }
    }
}
