namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement;

    public class ReceiptRecoveryRecoveryDateInFutureRule : IReceiptRecoveryContentRule
    {
        public ReceiptRecoveryRecoveryDateInFutureRule()
        {
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                List<int> shipments = new List<int>();
                MessageLevel result = MessageLevel.Success;

                foreach (var movement in movements.Where(p => !p.MissingRecoveredDisposedDate))
                {
                    if (movement.RecoveredDisposedDate > DateTime.Now || movement.RecoveredDisposedDate < movement.ReceivedDate)
                    {
                        result = MessageLevel.Error;
                        shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    }
                }

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.RecoveryDateValidation), shipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.RecoveryDateValidation, result, errorMessage);
            });
        }
    }
}