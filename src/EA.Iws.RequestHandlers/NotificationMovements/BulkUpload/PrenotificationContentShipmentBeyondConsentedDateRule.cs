namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationApplication.Shipment;

    public class PrenotificationContentShipmentBeyondConsentedDateRule : IBulkMovementPrenotificationContentRule
    {
        private readonly IShipmentInfoRepository repo;

        public PrenotificationContentShipmentBeyondConsentedDateRule(IShipmentInfoRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var shipmentInfo = await this.repo.GetByNotificationId(notificationId);

            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;

                int errorShipmentNumber = 0;

                foreach (var movement in movements.OrderBy(p => p.ActualDateOfShipment))
                {
                    if (movement.ActualDateOfShipment > shipmentInfo.ShipmentPeriod.LastDate)
                    {
                        missingDataResult = MessageLevel.Error;
                        errorShipmentNumber = movement.ShipmentNumber.GetValueOrDefault();
                        break;
                    }
                }

                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.BeyondConsentWindow), errorShipmentNumber);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.BeyondConsentWindow, missingDataResult, errorMessage);
            });
        }
    }
}