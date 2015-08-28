namespace EA.Iws.DocumentGeneration.Formatters
{
    using System;
    using Core.RecoveryInfo;
    using Domain.NotificationApplication;

    internal class RecoveryInfoFormatter
    {
        public string NullableDecimalAsPercentage(decimal? value)
        {
            if (value.HasValue)
            {
                return string.Format("{0} %", value.Value);
            }

            return string.Empty;
        }

        public string CostAmountWithUnits(RecoveryInfo recoveryInfo, Func<RecoveryInfo, decimal> cost,
            Func<RecoveryInfo, RecoveryInfoUnits> units)
        {
            if (recoveryInfo == null)
            {
                return string.Empty;
            }

            return string.Format("£ {0} / {1}", cost(recoveryInfo), units(recoveryInfo));
        }

        public string CostAmountWithUnits(RecoveryInfo recoveryInfo, Func<RecoveryInfo, decimal?> cost,
            Func<RecoveryInfo, RecoveryInfoUnits?> units)
        {
            if (recoveryInfo == null 
                || !cost(recoveryInfo).HasValue
                || !units(recoveryInfo).HasValue)
            {
                return string.Empty;
            }

            return string.Format("£ {0} / {1}", cost(recoveryInfo).Value, units(recoveryInfo).Value);
        }
    }
}
