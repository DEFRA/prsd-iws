namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationApplication;

    public class PrenotificationContentNotificationNumberRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public PrenotificationContentNotificationNumberRule(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var notificationNumber = await notificationApplicationRepository.GetNumber(notificationId);
            return await Task.Run(() =>
            {
                var notificationNumberResult = MessageLevel.Success;
                var notificationNumberShipmentNumbers = new List<string>();
                
                foreach (var movement in movements)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (movement.ShipmentNumber.HasValue && movement.NotificationNumber != notificationNumber)
                    {
                        notificationNumberResult = MessageLevel.Error;
                        notificationNumberShipmentNumbers.Add(movement.ShipmentNumber.ToString());
                    }
                }
                
                var shipmentNumbers = string.Join(", ", notificationNumberShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.WrongNotificationNumber), shipmentNumbers, notificationNumber);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.WrongNotificationNumber, notificationNumberResult, errorMessage);
            });
        }
    }
}
