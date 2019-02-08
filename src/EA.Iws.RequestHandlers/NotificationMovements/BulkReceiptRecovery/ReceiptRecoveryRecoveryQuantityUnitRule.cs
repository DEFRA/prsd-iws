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
    using Domain.NotificationApplication.Shipment;

    public class ReceiptRecoveryRecoveryQuantityUnitRule : IReceiptRecoveryContentRule
    {
        private readonly IShipmentInfoRepository repo;

        public ReceiptRecoveryRecoveryQuantityUnitRule(IShipmentInfoRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var shippingInfo = await repo.GetByNotificationId(notificationId);

            List<int> shipments = new List<int>();
            MessageLevel result = MessageLevel.Success;

            foreach (var movement in movements)
            {
                if (shippingInfo.Units != movement.Unit)
                {
                    result = MessageLevel.Error;
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                }
            }

            var shipmentNumbers = string.Join(", ", shipments);
            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.QuantityUnit), shipmentNumbers);

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.QuantityUnit, result, errorMessage);
        }
    }
}