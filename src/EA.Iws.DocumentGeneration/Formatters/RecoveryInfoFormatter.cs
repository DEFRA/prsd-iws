namespace EA.Iws.DocumentGeneration.Formatters
{
    using System;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;

    internal class RecoveryInfoFormatter
    {
        public string NullableDecimalAsPercentage(decimal? value)
        {
            if (value.HasValue)
            {
                return string.Format("{0}%", value.Value.ToString("G29"));
            }

            return string.Empty;
        }

        public string CostAmountWithUnits(RecoveryInfo recoveryInfo, Func<RecoveryInfo, ValuePerWeight> valuePerWeight)
        {
            if (recoveryInfo == null)
            {
                return string.Empty;
            }

            return string.Format("£{0} per {1}", valuePerWeight(recoveryInfo).Amount, valuePerWeight(recoveryInfo).Units);
        }

        public string CostAmountWithUnits(RecoveryInfo recoveryInfo, Func<RecoveryInfo, DisposalCost> disposalCost)
        {
            if (recoveryInfo == null || disposalCost(recoveryInfo) == null
                || !disposalCost(recoveryInfo).Amount.HasValue
                || !disposalCost(recoveryInfo).Units.HasValue)
            {
                return string.Empty;
            }

            return string.Format("£{0} per {1}", disposalCost(recoveryInfo).Amount, disposalCost(recoveryInfo).Units);
        }
    }
}
