﻿namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;

    public class ReceiptRecoveryReceiptDateFormatRule : IReceiptRecoveryContentRule
    {
        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(m => m.ShipmentNumber.HasValue && !m.MissingReceivedDate && !m.ReceivedDate.HasValue)
                        .GroupBy(x => x.ShipmentNumber)
                        .OrderBy(x => x.Key)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                var minShipment = shipments.FirstOrDefault() ?? 0;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.ReceiptDateFormat), shipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.ReceiptDateFormat, result, errorMessage, minShipment);
            });
        }
    }
}
