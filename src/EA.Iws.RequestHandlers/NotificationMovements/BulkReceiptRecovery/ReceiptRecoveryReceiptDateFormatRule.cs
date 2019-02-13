namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
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
                List<int> shipments = new List<int>();
                MessageLevel result = MessageLevel.Success;

                foreach (var movement in movements)
                {
                    if (!movement.MissingReceivedDate && movement.ReceivedDate == null)
                    {
                        result = MessageLevel.Error;
                        shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    }
                }

                var shipmentNumbers = string.Join(", ", shipments.Distinct());
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.ReceiptDateFormat), shipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.ReceiptDateFormat, result, errorMessage);
            });
        }
    }
}
